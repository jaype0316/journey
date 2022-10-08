import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Quote } from '../models/quote';

@Component({
  selector: 'app-quote',
  templateUrl: './quote.page.html',
  styleUrls: ['./quote.page.scss'],
})
export class QuotePage implements OnInit {

  quote:Quote;
  loaded:boolean;

  constructor(private http:HttpClient) { }

  ngOnInit() {
    this.getQuote();
  }

  async getQuote(){
    this.loaded = false;
    await this.http.get(environment.journeyApi + 'quote').subscribe((quote:Quote) =>{
      console.log('quote == ', quote);
      this.quote = quote;
    },(error:any) => {},
      () => {this.loaded = true;});
  }

}
