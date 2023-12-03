import { Component } from '@angular/core';
import { NewsService } from './services/news.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  newsStories:any;
  
  constructor(private newsService:NewsService){
    this.LoadStories('');
  }
  LoadStories(searchText:string)
  {
    this.newsService.GetStories(searchText).subscribe(result=>{
      this.newsStories = result;
    });
  }
  onSearchChange(searchText: string): void {  
    this.LoadStories(searchText); 
  }
  title = 'FrontEnd';
}
