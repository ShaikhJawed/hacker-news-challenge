import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class NewsService {
 url = 'https://localhost:44377/api/news';
  constructor(private httpClient:HttpClient) { }
  GetStories(searchText:any)
  {
     return this.httpClient.get(`${this.url}?searchText=${searchText}`);
  }

}
