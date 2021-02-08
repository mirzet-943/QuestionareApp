import { Routes, RouterModule } from '@angular/router';

import { HomeComponent } from './home';
import { LoginComponent } from './login';
import { NewsComponent } from './news/news.component';
import { WriterNewsComponent } from './news/writer.news.component';
import { UsersComponent } from './users/users.component';
import { AuthGuard } from './_helpers';

const routes: Routes = [
    { path: '', component: HomeComponent, canActivate: [AuthGuard] },
    { path: 'login', component: LoginComponent },
    { path: 'news', component: NewsComponent },
    { path: 'articles', component: WriterNewsComponent },
    { path: 'users', component: UsersComponent },

    // otherwise redirect to home
    { path: '**', redirectTo: '' }
];

export const appRoutingModule = RouterModule.forRoot(routes);