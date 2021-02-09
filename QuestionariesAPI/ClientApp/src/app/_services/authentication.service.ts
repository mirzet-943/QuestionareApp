import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { GoogleLoginProvider, SocialAuthService, SocialUser } from "angularx-social-login";
import { environment } from 'src/environments/environment';
import { User } from '../_models/user';
import { JwtHelperService } from '@auth0/angular-jwt';
import { ExternalAuth } from '../_models/ExternalAuth';
import { LoginResult } from '../_models/LoginResult';

@Injectable({ providedIn: 'root' })
export class AuthenticationService {
    private currentUserSubject: BehaviorSubject<User>;
    public currentUser: Observable<User>;
    public _externalAuth: ExternalAuth;

    constructor(private http: HttpClient, private _externalAuthService: SocialAuthService, private _jwtHelper : JwtHelperService) {
        this.currentUserSubject = new BehaviorSubject<User>(JSON.parse(localStorage.getItem('currentUser')));
        this.currentUser = this.currentUserSubject.asObservable();
    }

    public get currentUserValue(): User {
        return this.currentUserSubject.value;
    }

    public isUserAuthenticated = (): boolean => {
        const token = localStorage.getItem("token");
        return token && !this._jwtHelper.isTokenExpired(token);
      }

    login(username: string, password: string) {
        return this.http.post<any>(`${environment.apiUrl}/users/authenticate`, { username, password })
            .pipe(map(user => {
                // store user details and jwt token in local storage to keep user logged in between page refreshes
                localStorage.setItem('currentUser', JSON.stringify(user));
                localStorage.setItem("token", user.token);

                this.currentUserSubject.next(user);
                return user;
            }));
    }

    public externalLogin = (route: string, body: ExternalAuth) => {
        return this.http.post<LoginResult>(environment.apiUrl + route, body);
    }

    public logout() {
        this.currentUser = undefined;
        this.currentUserSubject = undefined;
        localStorage.removeItem('currentUser');
        localStorage.removeItem('token');
        this.currentUserSubject.next(null);
    }

    public signInWithGoogle = () => {
        return this._externalAuthService.signIn(GoogleLoginProvider.PROVIDER_ID);
    }
    
    public signOutExternal = () => {
        this._externalAuthService.signOut();
    }

    public setGoogleAuth(auth: ExternalAuth){
        this._externalAuth = auth;
    }
}