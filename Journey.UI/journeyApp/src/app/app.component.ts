import { Component, NgZone } from '@angular/core';
import { mergeMap } from 'rxjs/operators';
import { Browser } from '@capacitor/browser';
import { App } from '@capacitor/app';
import { AuthenticationService } from './services/auth.service';
import { Router } from '@angular/router';

const callbackUri = 'com.iter-meum://dev-2mb38pu2.us.auth0.com/capacitor/com.iter-meum/callback';

@Component({
  selector: 'app-root',
  templateUrl: 'app.component.html',
  styleUrls: ['app.component.scss'],
})
export class AppComponent {
  public appPages = [
    { title: 'Profile', url: '/tabs/profile', icon: 'person' },
    { title: 'About', url: '/tabs/about', icon: 'reader' },
    { title: 'Logout', url: '', icon: 'exit', onClick: this.clickLogout }
  ];
  public labels = [{title: 'About', url: '/tabs/about', icon: 'finger-print'}];
  constructor(private ngZone:NgZone, public auth: AuthenticationService, private router:Router) {}

  clickLogout(){
      this.auth.logout().subscribe(resp =>{
        this.router.navigate(["authenticate"]);
      });
  }

  ngOnInit():void {
     // Use Capacitor's App plugin to subscribe to the `appUrlOpen` event
     App.addListener('appUrlOpen', ({ url }) => {
      // Must run inside an NgZone for Angular to pick up the changes
      // https://capacitorjs.com/docs/guides/angular
      this.ngZone.run(() => {
        if (url?.startsWith(callbackUri)) {
          // If the URL is an authentication callback URL..
          if (
            url.includes('state=') &&
            (url.includes('error=') || url.includes('code='))
          ) {
            // Call handleRedirectCallback and close the browser
          } else {
            Browser.close();
          }
        }
      });
    });
  }
}
