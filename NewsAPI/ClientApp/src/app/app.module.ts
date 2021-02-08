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
import { MatDialogModule, MatTableModule  } from '@angular/material';
import { MatInputModule } from '@angular/material';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatToolbarModule } from '@angular/material/toolbar';

import { AppComponent } from './app.component';
import { appRoutingModule } from './app.routing';
import { JwtInterceptor, ErrorInterceptor } from './_helpers';
import { HomeComponent } from './home';
import { LoginComponent } from './login';
import { NewsComponent } from './news/news.component';
import { AddArticleDialog, EditArticleDialog, WriterNewsComponent } from './news/writer.news.component';
import { AddUserDialog, UsersComponent } from './users/users.component';
import { EditUserDialog } from './users/users.component';
import { FormsModule } from '@angular/forms';
import { MatFormFieldModule, MatProgressSpinnerModule } from '@angular/material';

@NgModule({
    imports: [
      BrowserAnimationsModule,
      MatDialogModule,
        BrowserModule,
        MatTableModule,
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
        MatInputModule
      ],
    declarations: [
        AppComponent,
        HomeComponent,
        LoginComponent,
        NewsComponent,
        UsersComponent,
        EditUserDialog,
        WriterNewsComponent,
        AddUserDialog,
        EditArticleDialog,
        AddArticleDialog
    ],
    entryComponents: [
      EditUserDialog,
      AddUserDialog,
      EditArticleDialog,
      AddArticleDialog
    ],
    providers: [
        { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
        { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true }

        // provider used to create fake backend
    ],
    bootstrap: [AppComponent]
})
export class AppModule { }