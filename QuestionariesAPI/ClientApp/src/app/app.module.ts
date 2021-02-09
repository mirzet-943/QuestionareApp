import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';

// used to create fake backend
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { MatMenuModule } from '@angular/material/menu';
import { MatDatepickerModule, MatDialogModule, MatNativeDateModule, MatSelectModule, MatTableModule  } from '@angular/material';
import { MatInputModule } from '@angular/material';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatToolbarModule } from '@angular/material/toolbar';

import { AppComponent } from './app.component';
import { appRoutingModule } from './app.routing';
import { JwtInterceptor, ErrorInterceptor } from './_helpers';
import { HomeComponent } from './home';
import { LoginComponent } from './login';
import { RegisterComponent } from './register';
import { AddArticleDialog, NewsComponent } from './news/questionaries.component';
import { AddUserDialog, UsersComponent } from './users/users.component';
import { EditUserDialog } from './users/users.component';
import { FormsModule } from '@angular/forms';
import { MatFormFieldModule, MatProgressSpinnerModule } from '@angular/material';
import { JwtModule } from "@auth0/angular-jwt";
import { SocialLoginModule, SocialAuthServiceConfig } from 'angularx-social-login';
import { GoogleLoginProvider } from 'angularx-social-login';
import { RECAPTCHA_V3_SITE_KEY, RecaptchaV3Module } from "ng-recaptcha";
export function tokenGetter() {
  return localStorage.getItem("token");
}

@NgModule({
    imports: [
      BrowserAnimationsModule,
      MatDialogModule,
        BrowserModule,
        MatSelectModule,
        MatDatepickerModule,
        MatNativeDateModule,
        MatTableModule,
        RecaptchaV3Module,
        MatProgressSpinnerModule,
        ReactiveFormsModule,
        FormsModule,
        HttpClientModule,
        appRoutingModule,
        MatButtonModule,
        MatMenuModule,
        MatCardModule,
        MatToolbarModule,
        MatIconModule,
        MatSidenavModule,
        MatListModule,
        MatFormFieldModule,
        MatInputModule,
        SocialLoginModule,
        JwtModule.forRoot({
          config: {
            tokenGetter: tokenGetter
          }
        })
      ],
    declarations: [
        AppComponent,
        HomeComponent,
        LoginComponent,
        NewsComponent,
        UsersComponent,
        EditUserDialog,
        AddUserDialog,
        AddArticleDialog,
        RegisterComponent
    ],
    entryComponents: [
      EditUserDialog,
      AddUserDialog,
      AddArticleDialog
    ],
    providers: [
        { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
        { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
        { provide: RECAPTCHA_V3_SITE_KEY, useValue: "6LcAKlAaAAAAAJUSdypVuMRTiDR0vZBlhkpeFqO1" },
        {
          provide: 'SocialAuthServiceConfig',
          useValue: {
            autoLogin: false,
            providers: [
              {
                id: GoogleLoginProvider.PROVIDER_ID,
                provider: new GoogleLoginProvider(
                  '401904579708-30eceiooba6ln9qbgjahngkf62ragq7r.apps.googleusercontent.com'
                )
              },
            ],
          } as SocialAuthServiceConfig
        }

        // provider used to create fake backend
    ],
    bootstrap: [AppComponent]
})
export class AppModule { }