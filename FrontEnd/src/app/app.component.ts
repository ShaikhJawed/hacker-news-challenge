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
  searchText:string = '';
  pageSize:number = 10;
  constructor(private newsService:NewsService){
    this.LoadStories();
  }
  LoadStories()
  {
    this.newsService.GetStories(this.searchText, this.pageSize).subscribe(result=>{
      this.apiResponse = result;
      this.newsStories= this.apiResponse.data;
    });
  }
  onSearchChange(value: string): void {  
    this.searchText = value;
    this.LoadStories(); 
  }
  getStoriesByUrl(uri:string){
    this.newsService.GetStoriesByUri(uri).subscribe(result=>{
      this.apiResponse = result;
      this.newsStories= this.apiResponse.data;
    });
  }
  onPageSizeChange(value:number)
  {
    this.pageSize = value;
    this.LoadStories()
  }
  title = 'FrontEnd';
}
