import { Component } from '@angular/core';
import { NewsService } from './services/news.service';
import { APIResponse } from './models/apiresponse.model';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  newsStories:any;
  apiResponse:APIResponse = new APIResponse();
  constructor(private newsService:NewsService){
    this.LoadStories('');
  }
  LoadStories(searchText:string)
  {
    this.newsService.GetStories(searchText).subscribe(result=>{
      this.apiResponse = result;
      this.newsStories= this.apiResponse.data;
    });
  }
  onSearchChange(searchText: string): void {  
    this.LoadStories(searchText); 
  }
  getStoriesByUrl(uri:string){
    this.newsService.GetStoriesByUri(uri).subscribe(result=>{
      this.apiResponse = result;
      this.newsStories= this.apiResponse.data;
    });
  }
  title = 'FrontEnd';
}
