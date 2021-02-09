import { Routes, RouterModule } from '@angular/router';

import { HomeComponent } from './home';
import { LoginComponent } from './login';
import { NewsComponent } from './news/questionaries.component';
import { RegisterComponent } from './register';
import { UsersComponent } from './users/users.component';
import { AuthGuard } from './_helpers';

const routes: Routes = [
    { path: '', component: LoginComponent, canActivate: [AuthGuard] },
    { path: 'login', component: LoginComponent },
    { path: 'questionare/:id', component: NewsComponent },
    { path: 'users', component: UsersComponent },
    { path: 'register', component: RegisterComponent },
    // otherwise redirect to home
    { path: '**', redirectTo: '' }
];

export const appRoutingModule = RouterModule.forRoot(routes);