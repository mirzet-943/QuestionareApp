import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { first } from 'rxjs/operators';

import { User } from '../_models/user';
import { UserService, AuthenticationService } from '../_services';

@Component({ templateUrl: 'home.component.html' })
export class HomeComponent {
    loading = false;
    constructor(public router:Router) { }

    ngOnInit() {
        this.loading = true;
         this.router.navigate(['/news'])
        
    }
}