import { ApplicationModule, Component, HostListener } from '@angular/core';
import { UsersApiService } from './users-api-service';
import { AppComponent } from 'src/app/app.component';
import { User } from '../_models/user';
import { MatDialog } from '@angular/material';
import {Inject} from '@angular/core';
import {MatDialogRef, MAT_DIALOG_DATA} from '@angular/material/dialog';
import { Router } from '@angular/router';


@Component({
  selector: 'app-root',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.css']
})
export class UsersComponent {

 
  // declare empty arrays for articles and news sources
  displayedColumns: string[] = ['Date of creation', 'Fullname', 'Username', 'Actions'];1
  mUsers: Array<any>;
  haveMorePages: boolean;
  nextPage: Number;
  searchTerm: String = '';
  isFetchingInProgress: boolean;
  currentUser: User;
  name: any;
  animal: any;
  
  constructor(private usersapi: UsersApiService, appComponent: AppComponent, public dialog: MatDialog,public router: Router) {
    console.log('users component constructor called');
    this.currentUser = appComponent.currentUser;
  }

// tslint:disable-next-line: use-life-cycle-interface
  ngOnInit() {
    // load articles
    if (this.currentUser ==undefined || this.currentUser.role  != "Admin")
      this.router.navigate(['/login'])
    this.isFetchingInProgress = true;
     this.usersapi.initUsers(this.searchTerm).subscribe(data => {this.mUsers = data['items']
        this.haveMorePages = data['hasNext'];
        this.nextPage = data["currentPage"] + 1;
    this.isFetchingInProgress = false;

     });
    }

  onKey(event: any) { // without type info
      this.searchTerm = (<HTMLInputElement>document.getElementById("searchTerm")).value;
        this.usersapi.initUsers(this.searchTerm).subscribe(data =>
           {
            this.mUsers = data['items']
            this.haveMorePages = data['hasNext'];
            this.nextPage = data["currentPage"] + 1;
            this.isFetchingInProgress = false;
           });
  }

  openDialog(element: any): void {
    const dialogRef = this.dialog.open(EditUserDialog, {
      width: '500px',
      data: {username: element.username, fullName: element.fullName},
    });

    dialogRef.afterClosed().subscribe(result => {
      console.log('The dialog was closed');
      if (result.username.length < 5 || result.password == undefined || result.password.length < 6 || result.fullName < 4)
      {
        alert("Please fill your data properly");
        return;
      }
         
      var user  =  {username: element.username, password: result.password, fullName: result.fullName, userID: element.userID};
      this.usersapi.putUser(user).subscribe(x=> {
        this.mUsers.forEach(el => {
          if (el.userID == element.userID){
             el.username = result.username;
             el.fullName = result.fullName;
            }
        });
      });
    });
  }

  openAddDialog(): void {
    const dialogRef = this.dialog.open(AddUserDialog, {
      width: '500px',
      data: {},
    });

    dialogRef.afterClosed().subscribe(result => {
      console.log('The dialog was closed');
      if (result.username.length < 5 || result.password == undefined || result.password.length < 6 || result.fullName < 4)
      {
        alert("Please fill your data properly");
        return;
      }
      var user  =  {username: result.username, password: result.password, fullName: result.fullName};
      this.usersapi.postUser(user).subscribe(x=> this.ngOnInit());
    });
  }

  deleteUser(element: any){
      if(confirm("Are you sure to delete user: " + element.username)) {
         this.usersapi.deleteUser(element).subscribe(s=>{
          this.usersapi.getNextPage(this.nextPage.toString(),this.searchTerm).subscribe(data =>{
            this.mUsers = (data['items'])
            this.haveMorePages = data['hasNext'];
            this.nextPage = data["currentPage"] + 1;
            this.isFetchingInProgress = false;
           }
          )});
      }
  }
  
  @HostListener("window:scroll", ["$event"])
  async onWindowScroll() {
  let pos = (document.documentElement.scrollTop || document.body.scrollTop) + document.documentElement.offsetHeight;
  let max = document.documentElement.scrollHeight;
  if(pos == max && this.haveMorePages)   {
    this.isFetchingInProgress = true;
            await this.delay(1500);
            this.usersapi.getNextPage(this.nextPage.toString(),this.searchTerm).subscribe(data =>{
            this.mUsers = this.mUsers.concat(data['items'])
            this.haveMorePages = data['hasNext'];
            this.nextPage = data["currentPage"] + 1;
            this.isFetchingInProgress = false;
           });
    }
  }

  // Putting delay because of fast api request (to show loading view only)
  delay(ms: number) {
    return new Promise( resolve => setTimeout(resolve, ms) );
  }
}

@Component({
  selector: 'user.edit.component',
  templateUrl: 'user.edit.component.html',
})
export class EditUserDialog {

    constructor(
      public dialogRef: MatDialogRef<EditUserDialog>,
      @Inject(MAT_DIALOG_DATA) public data: any) {
      }
  
    onNoClick(): void {
      this.dialogRef.close();
    }
}

@Component({
  selector: 'user.add.component',
  templateUrl: 'user.add.component.html',
})
export class AddUserDialog {

    constructor(
      public dialogRef: MatDialogRef<AddUserDialog>,
      @Inject(MAT_DIALOG_DATA) public data: any) {
      }
  
    onNoClick(): void {
      this.dialogRef.close();
    }
}
