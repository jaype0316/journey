import { HttpClient } from '@angular/common/http';
import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { IonInfiniteScroll } from '@ionic/angular';
import { Subscription } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Chapter } from '../models/chapter';
import { StorageService } from '../services/storage.service';

@Component({
  selector: 'app-chapters',
  templateUrl: './chapters.page.html',
  styleUrls: ['./chapters.page.scss'],
})
export class ChaptersPage implements OnInit {
 @ViewChild(IonInfiniteScroll) infiniteScroll: IonInfiniteScroll

  data: any[];
  pagedChapters: any[];
  skip = 0;
  take = 20;
  isLoading:boolean = false;
  tags = [];
  subscriptions: Subscription[];

  constructor(private http:HttpClient, private router:Router, private route:ActivatedRoute, private storage:StorageService) { 
    this.subscriptions = new Array();
  }

  ngOnInit() {   
  }

  ionViewDidEnter() {
    this.getChapters();
    this.getTags();
  }

  private getChapters(){
    this.isLoading = true;
    this.data = [];
    let chaptersRequest = this.http.get(environment.journeyApi + "Chapter/listheaders").subscribe((data:any[]) =>{
       let chapters = data;
       if(chapters && chapters.length){
        this.data = [];
        this.data.unshift(...chapters);
        this.pageChapters(true, null);
        this.sortChapters();
       } 
    }, (error:any) =>{

    }, () => {
      this.isLoading = false;
    });
    this.subscriptions.push(chaptersRequest);
  }

  private getTags(){
    let tagsRequest = this.http.get(environment.journeyApi + "tags").subscribe((tags:any[]) => {
      this.tags = [];
      tags.map((val,index) => {
        this.tags.push({ label: val.name, isActive: false});
      });
      this.tags.unshift({label: 'All', isActive: true})
      console.log('tags == ', this.tags);
    },(err) =>{}, () => {}); 

    this.subscriptions.push(tagsRequest);
  }

  pageChapters(isInitialLoad: boolean, event) {
    if(isInitialLoad){
      this.pagedChapters = this.data.slice(0,this.take);
      return;
    }
    
    if(this.pagedChapters.length == this.data.length) {
      event.target.disable = true;
      event.target.complete();
    }
    else {
      this.wait(500);
      event.target.complete();
      let nextPaginatedChapters = this.data.slice(this.skip,this.take);
      this.pagedChapters.push(...nextPaginatedChapters);
      //load 5 more next time
      this.skip += 5;
    }
  }

   loadData(event) {
    this.pageChapters(false,event);
  }


  clickGoToChapter(chapter:any){
    this.router.navigate(['tabs/chapter',chapter.pk]);
  }

  private wait(time) {
    return new Promise<void>((resolve) => {
      setTimeout(() => {
        resolve();
      }, time);
    });
  }

  private sortChapters() {
    this.pagedChapters.sort((a,b) => b.createdAt - a.createdAt);
    console.log('paged chapters == ', this.pagedChapters);
  }

  searchInputChanged($event){
    const query = $event.target.value.toLowerCase();
    console.log('query from search bar == ', query);
    
    if(query && query.length < 2)
       return;
    if(!this.pagedChapters || this.pageChapters.length == 0)
      return;

      this.pagedChapters =  this.data.filter((value, index) => {
        return (value.title.toLowerCase().indexOf(query) > -1);
      });
  }

  clickTag(tag){
    console.log('tag clicked == ', tag);
    tag.isActive = !tag.isActive;
  
    if(tag.label == 'All'){
      this.deselectTags();
      this.pageChapters(true, null);
      return;
    }

    //if none are currently active
    const allAreInactive = this.tags.every((value, index, arr) =>{
      return value.isActive == false;
    });
    if(allAreInactive){
      this.tags[0].isActive = true;
      this.pageChapters(true, null);
      return;
    }

    //deselect 'All'
    this.tags[0].isActive = false;
    const activeTags = this.getActiveTags();
    this.pagedChapters =  this.data.filter((chapter, index) => {
      let chapterTags = chapter.tags.map(t => t.name) || [];
      let intersection = activeTags.filter((tag) => chapterTags.includes(tag.label));
      return intersection.length > 0;
    });
  }

  private getActiveTags(){
    return this.tags.filter((tag, index) => { return tag.isActive});
  }

  private deselectTags(){
    for(var i = 1; i < this.tags.length - 1; i++){
      this.tags[i].isActive = false;
    }
  }

  searchInputCanceled(){
    this.pageChapters(true, null);
  }

  ionViewWillLeave(){
    this.subscriptions.forEach(s => s.unsubscribe());
  }

}
