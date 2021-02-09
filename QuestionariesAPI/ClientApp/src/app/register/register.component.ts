import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { first } from 'rxjs/operators';
import { AuthenticationService } from '../_services/authentication.service';
import { SocialUser } from 'angularx-social-login';
import { ExternalAuth } from '../_models/ExternalAuth';
import { error } from 'protractor';

@Component({ templateUrl: 'register.component.html' })
export class RegisterComponent implements OnInit {
    loginForm: FormGroup;
    loading = false;
    submitted = false;
    returnUrl: string;
    error = '';
    username: string;

    constructor(
        private formBuilder: FormBuilder,
        private route: ActivatedRoute,
        private router: Router,
        private authenticationService: AuthenticationService
    ) { 
        // redirect to login if token empty
        if (!this.authenticationService._externalAuth) { 
            this.router.navigate(['/login']);
        }
        this.username = authenticationService._externalAuth.email;
    }

    ngOnInit() {
        this.loginForm = this.formBuilder.group({
            username: [''],
            password: ['', Validators.required],
            repeat_password: ['', Validators.required]
        },{Validators: this.checkPasswords});

        // get return url from route parameters or default to '/'
        this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/users';
    }

    checkPasswords(group: FormGroup) { // here we have the 'passwords' group
        const password = group.get('password').value;
        const confirmPassword = group.get('repeat_password').value;
        return password === confirmPassword ? null : { notSame: true }     
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
        this.setupPassword();
    }

    public setupPassword() {
      this.authenticationService._externalAuth.password = this.f.password.value;
      this.validateExternalAuth(this.authenticationService._externalAuth);
    }
    
    private validateExternalAuth(externalAuth: ExternalAuth) {
        this.authenticationService.externalLogin('/users/externallogin', externalAuth)
          .subscribe(res => {
            this.loading = false;
            if (res.isAuthSuccess && !res.isPasswordSetupNeeded){
              localStorage.setItem("token", res.user.token);
              this.router.navigate([this.returnUrl]);
            }
            this.error = "Internal server error";
          },
          error => {
            this.error = error;
            this.loading = false;
          });
    }
}
