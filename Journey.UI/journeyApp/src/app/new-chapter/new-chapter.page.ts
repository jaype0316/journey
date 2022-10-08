import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Chapter } from '../models/chapter';
import { StorageService } from '../services/storage.service';
import { ToastController } from '@ionic/angular';
import { Router } from '@angular/router';

@Component({
  selector: 'app-new-chapter',
  templateUrl: './new-chapter.page.html',
  styleUrls: ['./new-chapter.page.scss'],
})
export class NewChapterPage implements OnInit {
  
  chapter:Chapter;
  isBusy:boolean = false;
  //todo: move out of here
  tags = ['Perseverance', 'Resiliency', 'Commitment', 'Sacrifice'];

  constructor(private storage: StorageService, private http:HttpClient, private toastr: ToastController, private router:Router) { }

  ngOnInit() {
    this.chapter = new Chapter();
  }

  changeTags($event){
    console.log('tags == ', $event.detail.value);
    this.chapter.tags = $event.detail.value;
  }

  async clickSaveAndFinish() {
    console.log('chapter == ',  this.chapter);
    this.isBusy =true;
    this.http.post(environment.journeyApi + "Chapter/Save", this.chapter).subscribe((pk:any) => {
      if(this.chapter.pk == null) 
        this.chapter.pk = pk;

        this.chapter = new Chapter();
        this.showToast('Saved');
        this.router.navigate(['tabs/chapters']);
    }, (error:any) =>{
      this.showToast('Error saving');
    }, () => {this.isBusy = false;});
  }

  private randomId(maxLimit = 100) {
    let rand = Math.random() * maxLimit;
    return Math.floor(rand);
   }

   isPageValid(){
     return !!(this.chapter && this.chapter.title && this.chapter.body);
   }

   private async showToast(message:string){
    const toast = await this.toastr.create({
      message:message,
      duration: 2000
    });
    toast.present();
  }

}
