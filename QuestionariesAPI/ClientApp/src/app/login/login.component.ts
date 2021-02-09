import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { first } from 'rxjs/operators';
import { AuthenticationService } from '../_services/authentication.service';
import { SocialUser } from 'angularx-social-login';
import { ExternalAuth } from '../_models/ExternalAuth';

@Component({ templateUrl: 'login.component.html' })
export class LoginComponent implements OnInit {
    loginForm: FormGroup;
    loading = false;
    submitted = false;
    returnUrl: string;
    error = '';

    constructor(
        private formBuilder: FormBuilder,
        private route: ActivatedRoute,
        private router: Router,
        private authenticationService: AuthenticationService
    ) { 
        // redirect to home if already logged in
        if (this.authenticationService.currentUserValue && this.authenticationService.isUserAuthenticated()) { 
            this.router.navigate(['/users']);
        }
    }

    ngOnInit() {
        this.loginForm = this.formBuilder.group({
            username: ['', Validators.required],
            password: ['', Validators.required]
        });

        // get return url from route parameters or default to '/'
        this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/users';
    }

    // convenience getter for easy access to form fields
    get f() { return this.loginForm.controls; }

    onSubmit() {
        this.submitted = true;

        // stop here if form is invalid
        if (this.loginForm.invalid) {
            return;
        }

        this.loading = true;
        this.authenticationService.login(this.f.username.value, this.f.password.value)
            .pipe(first())
            .subscribe(
                data => {
                    this.router.navigate([this.returnUrl]);
                },
                error => {
                    this.error = error;
                    this.loading = false;
                });
    }

    public externalLogin = () => {
      this.authenticationService.setGoogleAuth(undefined);
        this.authenticationService.signInWithGoogle()
        .then(res => {
          const user: SocialUser = { ...res };
          console.log(user);
          const externalAuth: ExternalAuth = {
            provider: user.provider,
            idToken: user.idToken,
            password: '',
            email : user.email
          }
          this.authenticationService.setGoogleAuth(externalAuth);
          this.validateExternalAuth(externalAuth);
        }, error => console.log(error))
      }

      private validateExternalAuth(externalAuth: ExternalAuth) {
        this.authenticationService.externalLogin('/users/externallogin', externalAuth)
          .subscribe(res => {
            if (res.isAuthSuccess && !res.isPasswordSetupNeeded){
              localStorage.setItem("token", res.user.token);
              this.router.navigate([this.returnUrl]);
            }else{
              this.router.navigate(["/register"]);
            }
          },
          error => {
            this.error = error;
            this.authenticationService.signOutExternal();
            this.loading = false;
          });
    }
    
}
