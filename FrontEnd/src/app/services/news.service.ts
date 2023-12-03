import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { APIResponse } from '../models/apiresponse.model';

@Injectable({
  providedIn: 'root'
})
export class NewsService {
 url = 'https://localhost:44377/api/news';
  constructor(private httpClient:HttpClient) { }
  GetStories(searchText:any, pageSize:number)
  {
     return this.httpClient.get<APIResponse>(`${this.url}?searchText=${searchText}&pageSize=${pageSize}`);
  }
  GetStoriesByUri(uri:string)
  {
     return this.httpClient.get<APIResponse>(uri);
  }

}
