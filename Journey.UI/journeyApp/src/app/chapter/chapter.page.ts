import { Component, OnInit, ViewChild, ChangeDetectorRef } from '@angular/core';
import { ActivatedRoute, ParamMap, Router } from '@angular/router';
import { Chapter } from '../models/chapter';
import { StorageService } from '../services/storage.service';
import { SwiperComponent } from "swiper/angular";
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';
import { Location } from '@angular/common';
import SwiperCore, {Pagination, Navigation} from "swiper";

SwiperCore.use([Pagination, Navigation])

@Component({
  selector: 'app-chapter',
  templateUrl: './chapter.page.html',
  styleUrls: ['./chapter.page.scss'],
})
export class ChapterPage implements OnInit {
  @ViewChild('swiper', { static: false }) swiper?: SwiperComponent;
  
  constructor(private storage:StorageService, private route:ActivatedRoute, private http:HttpClient,
    private router: Router, private location:Location, private changeDetection:ChangeDetectorRef) { }

  private slides:any;
  chapters:Chapter[]
  currentChapter:Chapter = null;
  loading:any = {};
  private chapterId:string;

  ngOnInit() {
    this.loading.chapters = true;

    this.http.get(environment.journeyApi + "Chapter/list").subscribe((data:any[]) =>{
      this.chapters = data;
      console.log('chapters == ', this.chapters);
      
      this.route.paramMap.subscribe((params: ParamMap) =>  {
        this.chapterId = params.get('id');
        this.getChapter(this.chapterId).subscribe((chapter:Chapter) => {
            this.currentChapter = chapter;
            console.log('current chapter  from route sub == ', this.currentChapter);
            this.setActiveSlide();
        });
      });
    }, 
    (error:any) => {},
    () => { this.loading.chapters = false;} );
  }

  ionViewDidEnter() {
    
  }

  private getChapter(chapterId:string):Observable<Chapter>{
    return this.http.get<Chapter>(environment.journeyApi + "chapter/get/" + chapterId);
  }

  private setActiveSlide(){
    let index = this.chapters.findIndex((val,idx) => {return val.pk == this.currentChapter.pk});
    console.log('swiper instance found index == ', index);
    this.swiper.swiperRef.slideTo(index, 200, false);
  }

  getActiveChapter():Chapter {
    let chapterIndex = this.swiper.swiperRef.activeIndex;
    let targetChapter = this.chapters[chapterIndex];
    return targetChapter;
  }

  clickEdit(){
    this.router.navigate(['tabs/chapter/update',this.currentChapter.pk]);
  }

  trackTag(index:number, tag:any){
    return tag;
  }

/*
swiper events

*/
  
  slideChange() {
    let chapterIndex = this.swiper.swiperRef.activeIndex;
    console.log('active index = ', chapterIndex);
    this.currentChapter = this.chapters[chapterIndex];
    this.changeDetection.detectChanges();
    console.log('current chapter == ', this.currentChapter);
  }

  slideChangeTransitionEnd(){
   
  }

  slideChangeTransitionStart(){}
  doubleTap(){}
  sliderMove(){}
  // slideNextTransitionStart() {
  //   this.updateRouteForCurrentSlide();
  // };
  // slidePrevTransitionStart() {
  //   this.updateRouteForCurrentSlide();
  // }

  slideNextTransitionEnd(){}
  slidePrevTransitionEnd(){}
  tap(){}
  touchStart(){}
  touchEnd(){}
  transitionStart(){}
  transitionEnd(){}
  init(){}
  reachBeginning(){}
  reachEnd(){}

}
