import { HttpClient } from '@angular/common/http';
import { Component, OnInit, Renderer2 } from '@angular/core';
import { BooleanValueAccessor } from '@ionic/angular';
import { environment } from 'src/environments/environment';
import { StorageService } from '../services/storage.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.page.html',
  styleUrls: ['./profile.page.scss'],
})
export class ProfilePage implements OnInit {

  defaultTheme:string = 'light';
  profile:any;
  profileJson: string = null;
  isBusy: boolean;
  constructor(private http:HttpClient, private storage: StorageService, private rendered: Renderer2) { }

  ngOnInit() {
    this.isBusy = true;
    this.http.get(environment.journeyApi + "Account/UserProfile").subscribe(up =>{
      this.profile = up;
      console.log('profile == ', this.profile);
    }, (err) => {}, () => { this.isBusy = false;});
    // this.auth.user$.subscribe(
    //   (profile) => {
    //       this.profileJson = JSON.stringify(profile, null, 2);
    //       console.log('profile json == ', this.profileJson);
    //   });

    // this.profile = {
    //   name: 'juan hernandez',
    //   createdAt: new Date().toDateString()
    // };
  }

  themeSelectionChanged(e){
    this.defaultTheme = e.detail.value;
    this.storage.save('theme', this.defaultTheme);
    this.rendered.setAttribute(document.body, 'color-theme', this.defaultTheme);
  }

}
