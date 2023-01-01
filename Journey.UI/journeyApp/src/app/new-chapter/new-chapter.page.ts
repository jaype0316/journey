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
  soundtracks = [];
  currentSoundtrack:any;
  audioActive:boolean = false;
  audio = new Audio();

  constructor(private storage: StorageService, private http:HttpClient, private toastr: 
    ToastController, private router:Router) { }

  ngOnInit() {
    this.chapter = new Chapter();
    this.getSoundtracks();
  }

  changeTags($event){
    console.log('tags == ', $event.detail.value);
    this.chapter.tags = $event.detail.value;
  }

  async getSoundtracks(){
    this.http.get(environment.journeyApi + "soundtrack/download").subscribe((soundtracks:any[]) => {
        this.soundtracks = soundtracks.map((uri, index) => {
          return {uri:uri, index:index};
        });
        console.log('soundtracks == ', this.soundtracks);
        this.randomizeNextSoundtrack();
    });
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
     // this.showToast('Error saving');
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

  private randomizeNextSoundtrack(): any {
    const randomIndex = Math.floor(Math.random() * this.soundtracks.length);
    return this.soundtracks[randomIndex];
    // let nextSoundtrack = this.soundtracks[randomIndex];
    // return nextSoundtrack
    // this.currentSoundtrack = this.media.create(nextSoundtrackUri);

    // this.currentSoundtrack.onStatusUpdate.subscribe(status => console.log('sound player status == ', status));
    // this.currentSoundtrack.onSuccess.subscribe(() => {'sound player on success'});
    // this.currentSoundtrack.onError.subscribe(error => console.log('Error!', error)); 
  }

  clickPlay(){
    console.log('click play');
    this.audioActive = !this.audioActive;
    if(!this.audioActive){
      this.audio.pause();
    } else {
      this.play();
    }
    //this.audioPlayer.play(this.randomizeNextSoundtrack());
    //this.currentSoundtrack.play();
  }

  private play(){
    this.currentSoundtrack = this.currentSoundtrack || this.randomizeNextSoundtrack();
        
    this.audio.src = this.currentSoundtrack.uri;
    this.audio.load();
    this.audio.play();
  }

  clickShuffle($event){
    this.currentSoundtrack = this.randomizeNextSoundtrack();
    this.audioActive = !this.audioActive;
    this.play();
  }

  pause(){
    //this.currentSoundtrack.pause();
    //this.audioPlayer.pause();
  }

  stop(){
    //this.currentSoundtrack.stop();
  }



}
