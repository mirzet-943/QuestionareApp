import { Injectable } from '@angular/core';
import { HttpClient  } from '@angular/common/http';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class UsersApiService {


  // Add your API key here
  api_key = 'YOUR API KEY';

  constructor(private http: HttpClient) { }

  // function to get list of all news sources
  initSources() {
     return this.http.get(`${environment.apiUrl}/users?pageNumber=1&searchTerm=`);
  }


  // function to get a list of headlines
  initUsers(searchTerm: String) {
    return this.http.get(`${environment.apiUrl}/users?pageNumber=1&searchTerm=${searchTerm}`);
  }

  // function to get a list of headlines for a user-selected source
  getUsersByID(source: String) {
   return this.http.get(`${environment.apiUrl}/users/${source}`);
  }
  
   getNextPage(page: String, searchTerm: String) {
    return this.http.get(`${environment.apiUrl}/users?pageNumber=${page}&searchTerm=${searchTerm}`);
   }

   putUser(element: any) {
    return this.http.put(`${environment.apiUrl}/users/edit/${element.userID}`,element);
   }
   postUser(element: any) {
    return this.http.post(`${environment.apiUrl}/users/register`,element);
   }

   deleteUser(element: any) {
    return this.http.delete(`${environment.apiUrl}/users/delete/${element.userID}`,element);
   }
}
