import { Component, HostListener } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthenticationService } from '../_services';
import { NewsApiService } from './news-api.service';


@Component({
  selector: 'app-root',
  templateUrl: './news.component.html',
  styleUrls: ['./news.component.css']
})
export class NewsComponent {

  // declare empty arrays for articles and news sources
  mArticles: Array<any>;
  mSources: Array<any>;
  haveMorePages: boolean;
  nextPage: Number;
  searchTerm: String = '';
  isFetchingInProgress: boolean;
  isAdmin:boolean;
  isWriter:boolean;
  constructor(private newsapi: NewsApiService, public auth: AuthenticationService, private router: Router) {
    console.log('news component constructor called');
  }


// tslint:disable-next-line: use-life-cycle-interface
  ngOnInit() {
    this.auth.currentUser.subscribe(s=> {this.isAdmin = s.role == "Admin"; this.isWriter = s.role == "Writer"});
    // load articles
    this.isFetchingInProgress = true;
        this.newsapi.initArticles(this.searchTerm).subscribe(data => {this.mArticles = data['items']
        this.haveMorePages = data['hasNext'];
        this.nextPage = data["currentPage"] + 1;
        this.isFetchingInProgress = false;
     });
    }

  // function to search for articles based on a news source (selected from UI mat-menu)
  likeArticle(articleId) {
    this.newsapi.likeArticle(articleId).subscribe(data => this.mArticles.forEach(element => {
      if (element.articleId == articleId)
          element.likes++;
    }));
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
