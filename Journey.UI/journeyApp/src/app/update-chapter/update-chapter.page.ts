import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, ParamMap, Router } from '@angular/router';
import { ToastController } from '@ionic/angular';
import { Subscription } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Chapter } from '../models/chapter';
import { Tag } from '../models/tag';

@Component({
  selector: 'app-update-chapter',
  templateUrl: './update-chapter.page.html',
  styleUrls: ['./update-chapter.page.scss'],
})
export class UpdateChapterPage implements OnInit {
  loaded:boolean = false;
  chapter:Chapter;
  tags:Tag[];
  tagsSelected:Tag[];
  subscriptions: Subscription[];

  constructor(private http:HttpClient, private route:ActivatedRoute, private toastr: ToastController, private router:Router) { 
    this.subscriptions = new Array();
    this.tagsSelected = new Array();
  }

  ngOnInit() {
    this.route.paramMap.subscribe((params: ParamMap) =>  {
      let chapterId = params.get('id');
      this.getChapter(chapterId);    
    });
  }

  ionViewDidEnter() {
    this.getTags();
  }

  private getChapter(chapterId:string){
    this.loaded = false;
    const chapterRequest = this.http.get(environment.journeyApi + "chapter/get/" + chapterId).subscribe((chapter:Chapter) => {
      this.chapter = chapter;
      this.loaded = true;
      console.log('current chapter == ', this.chapter);
      this.tagsSelected = this.chapter.tags;
    }, (error:any) => {
    });

    this.subscriptions.push(chapterRequest);
  }

  private getTags(){
    let tagsRequest = this.http.get(environment.journeyApi + "tags").subscribe((tags:any[]) => {
      this.tags = tags;
      console.log('tags == ', this.tags);
    },(err) =>{}, () => {}); 

    this.subscriptions.push(tagsRequest);
  }

  clickSave(){
    this.chapter.tags = this.tagsSelected;
    const saveRequest = this.http.post(environment.journeyApi + "chapter/Save", this.chapter).subscribe((resp:any) => {
        this.showToast('Saved');
        this.router.navigate(['tabs/chapters']);
    });

    this.subscriptions.push(saveRequest);
  }

  changeTags($event){
    console.log('tags == ', $event.detail.value);
    this.chapter.tags = $event.detail.value;
  }

  async showToast(message:string){
    const toast = await this.toastr.create({
      message:message,
      duration: 2000
    });
    toast.present();
  }

  compareWith(t1, t2){
    return t1 && t2 ? t1.id === t2.id : t1 === t2;
  }

  changeSelectedTags(evt){
    this.tagsSelected = evt.target.value;
  }

  ionViewWillLeave(){
    this.subscriptions.forEach(s => s.unsubscribe());
  }

}
