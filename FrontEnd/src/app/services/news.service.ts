import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { APIResponse } from '../models/apiresponse.model';

@Injectable({
  providedIn: 'root'
})
export class NewsService {
 url = 'https://localhost:44377/api/news';
  constructor(private httpClient:HttpClient) { }
  GetStories(searchText:any)
  {
     return this.httpClient.get<APIResponse>(`${this.url}?searchText=${searchText}`);
  }
  GetStoriesByUri(uri:string)
  {
     return this.httpClient.get<APIResponse>(uri);
  }

}
