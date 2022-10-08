import { Component, OnInit, Renderer2 } from '@angular/core';
import { AuthService } from '@auth0/auth0-angular';
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
  constructor(private storage: StorageService, private rendered: Renderer2, public auth:AuthService) { }

  ngOnInit() {
    this.auth.user$.subscribe(
      (profile) => {
          this.profileJson = JSON.stringify(profile, null, 2);
          console.log('profile json == ', this.profileJson);
      });

    this.profile = {
      name: 'juan hernandez',
      createdAt: new Date().toDateString()
    };
  }

  themeSelectionChanged(e){
    this.defaultTheme = e.detail.value;
    this.storage.save('theme', this.defaultTheme);
    this.rendered.setAttribute(document.body, 'color-theme', this.defaultTheme);
  }

}
