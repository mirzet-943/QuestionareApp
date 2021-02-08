import { Component, HostListener, Inject } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { AuthenticationService } from '../_services';
import { WriterArticlesService } from './writer.articles.service';


@Component({
  selector: 'app-root',
  templateUrl: './news.component.html',
  styleUrls: ['./news.component.css']
})
export class WriterNewsComponent {


  mArticles: Array<any>;
  mSources: Array<any>;
  haveMorePages: boolean;
  nextPage: Number;
  searchTerm: String = '';
  isFetchingInProgress: boolean;
  isAdmin:boolean;
  userId:Number;
  isWriter: boolean;
  writerArticlesOnly:boolean = true;
  constructor(private newsapi: WriterArticlesService, public auth: AuthenticationService,public dialog: MatDialog) {
    console.log('news component constructor called');
  }

// tslint:disable-next-line: use-life-cycle-interface
  ngOnInit() {
    this.auth.currentUser.subscribe(s=>{this.isAdmin = s.role == "Admin";this.userId = s.userID; ; this.isWriter = s.role == "Writer" });
    // load articles
    this.isFetchingInProgress = true;
     this.newsapi.initArticles(this.searchTerm).subscribe(data => {this.mArticles = data['items']
        this.haveMorePages = data['hasNext'];
        this.nextPage = data["currentPage"] + 1;
        this.isFetchingInProgress = false;
     });
    }

  deleteArticle(articleId) {

    this.newsapi.deleteArticle(articleId).subscribe(data => {
    this.mArticles.forEach(element => {
      if (element.articleId == articleId){
          this.mArticles = this.mArticles.filter (s=>s != element);
          return;
      }
    });
     })
  }

  openAddDialog(): void {
    const dialogRef = this.dialog.open(AddArticleDialog, {
      width: '500px',
      data: {},
    });

    dialogRef.afterClosed().subscribe(result => {
      console.log('The dialog was closed');
      if (result.title.length < 5 || result.content == undefined || result.content.length < 5)
      {
        alert("Please fill your data properly");
        return;
      }
      var article  =  {subject: result.title, text: result.content};
      this.newsapi.postArticle(article).subscribe(x=> this.ngOnInit());
    });
  }

  openEditDialog(element: any): void {
    const dialogRef = this.dialog.open(EditArticleDialog, {
      width: '500px',
      data: {content: element.text, title: element.subject},
    });

    dialogRef.afterClosed().subscribe(result => {
      console.log('The dialog was closed');
      if (result.title.length < 5 || result.content == undefined || result.content.length < 5)
      {
        alert("Please fill your data properly");
        return;
      }
         
      var article  =  {subject: result.title, text: result.content, articleId: element.articleId};
      this.newsapi.editArticle(article).subscribe(x=> {
        this.mArticles.forEach(el => {
          if (el.articleId == element.articleId){
             el.subject = result.title;
             el.text = result.content;
            }
        });
      });
    });
  }

  onKey(event: any) { // without type info
      this.searchTerm = (<HTMLInputElement>document.getElementById("searchTerm")).value;
      this.newsapi.initArticles(this.searchTerm).subscribe(data => {this.mArticles = data['items']
      this.haveMorePages = data['hasNext'];
      this.nextPage = data["currentPage"] + 1;
      this.isFetchingInProgress = false;
   });
  }
  
  @HostListener("window:scroll", ["$event"])
  async onWindowScroll() {
  let pos = (document.documentElement.scrollTop || document.body.scrollTop) + document.documentElement.offsetHeight;
  let max = document.documentElement.scrollHeight;
  if(pos == max && this.haveMorePages)   {
    this.isFetchingInProgress = true;
            await this.delay(1500);
            this.newsapi.getNextPage(this.nextPage.toString(),this.searchTerm).subscribe(data =>{
            this.mArticles = this.mArticles.concat(data['items'])
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
  selector: 'edit.article.dialog',
  templateUrl: '/dialogs/edit.article.dialog.html',
})
export class EditArticleDialog {

    constructor(
      public dialogRef: MatDialogRef<EditArticleDialog>,
      @Inject(MAT_DIALOG_DATA) public data: any) {
      }
  
    onNoClick(): void {
      this.dialogRef.close();
    }
}

@Component({
  selector: 'add.article.dialog',
  templateUrl: '/dialogs/add.article.dialog.html',
})
export class AddArticleDialog {

    constructor(
      public dialogRef: MatDialogRef<AddArticleDialog>,
      @Inject(MAT_DIALOG_DATA) public data: any) {
      }
  
    onNoClick(): void {
      this.dialogRef.close();
    }
}
