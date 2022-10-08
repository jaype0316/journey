import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, ParamMap, Router } from '@angular/router';
import { ToastController } from '@ionic/angular';
import { environment } from 'src/environments/environment';
import { Chapter } from '../models/chapter';

@Component({
  selector: 'app-update-chapter',
  templateUrl: './update-chapter.page.html',
  styleUrls: ['./update-chapter.page.scss'],
})
export class UpdateChapterPage implements OnInit {
  loaded:boolean = false;
  chapter:Chapter;

  //todo: move out of here
  tags = ['Perseverance', 'Resiliency', 'Commitment', 'Sacrifice'];
  constructor(private http:HttpClient, private route:ActivatedRoute, private toastr: ToastController, private router:Router) { }

  ngOnInit() {
    this.route.paramMap.subscribe((params: ParamMap) =>  {
      let chapterId = params.get('id');
      this.getChapter(chapterId);    
    });
  }

  private getChapter(chapterId:string){
    this.loaded = false;
    this.http.get(environment.journeyApi + "chapter/get/" + chapterId).subscribe((chapter:Chapter) => {
      this.chapter = chapter;
      this.loaded = true;
      console.log('current chapter == ', this.chapter);
    }, (error:any) => {
    });
  }

  clickSave(){
    this.http.post(environment.journeyApi + "chapter/Save", this.chapter).subscribe((resp:any) => {
        this.showToast('Saved');
        this.router.navigate(['tabs/chapters']);
    });
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

}
