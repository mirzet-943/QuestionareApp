import { Injectable } from '@angular/core';
import { HttpClient  } from '@angular/common/http';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class NewsApiService {


  // Add your API key here
  api_key = 'YOUR API KEY';

  constructor(private http: HttpClient) { }

  // function to get a list of headlines for a user-selected source
  initQuestionare(id: String, body: any) {
   return this.http.post(`${environment.apiUrl}/questionare/${id}`, body);
  }

  submitQuestionare(id: String, body: any) {
    return this.http.post(`${environment.apiUrl}/questionare/${id}/submit`, body);
   }
   
}
