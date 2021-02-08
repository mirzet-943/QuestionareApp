import { Injectable } from '@angular/core';
import { HttpClient  } from '@angular/common/http';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class WriterArticlesService {


  // Add your API key here
  api_key = 'YOUR API KEY';

  constructor(private http: HttpClient) { }

  // function to get list of all news sources
  initSources() {
     return this.http.get(`${environment.apiUrl}/articles?myArticles=true&pageNumber=1`);
  }

  // function to get a list of headlines
  initArticles(searchTerm: String) {
    return this.http.get(`${environment.apiUrl}/articles?myArticles=true&pageNumber=1&searchTerm=${searchTerm}`);
  }
  
   getNextPage(page: String, searchTerm: String) {
    return this.http.get(`${environment.apiUrl}/articles/?myArticles=true&pageNumber=${page}&searchTerm=${searchTerm}`);
   }

   deleteArticle(articleId: String) {
    return this.http.delete(`${environment.apiUrl}/articles/${articleId}`);
   }

   editArticle(article: any) {
    return this.http.put(`${environment.apiUrl}/articles/edit/${article.articleId}`,article);
   }

   postArticle(article: any) {
    return this.http.post(`${environment.apiUrl}/articles/create`,article);
   }
}
