import { Component, NgZone } from '@angular/core';
import { AuthService } from '@auth0/auth0-angular';
import { mergeMap } from 'rxjs/operators';
import { Browser } from '@capacitor/browser';
import { App } from '@capacitor/app';

const callbackUri = 'com.iter-meum://dev-2mb38pu2.us.auth0.com/capacitor/com.iter-meum/callback';

@Component({
  selector: 'app-root',
  templateUrl: 'app.component.html',
  styleUrls: ['app.component.scss'],
})
export class AppComponent {
  public appPages = [
    { title: 'Chapers', url: '/tabs/chapters', icon: 'folder' },
    { title: 'Quote', url: '/tabs/quote', icon: 'reader' },
    { title: 'Profile', url: '/tabs/profile', icon: 'person' }
  ];
  public labels = [{title: 'About', url: '/tabs/about', icon: 'finger-print'}];
  constructor(public auth:AuthService, private ngZone:NgZone) {}

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
            this.auth
              .handleRedirectCallback(url)
              .pipe(mergeMap(() => Browser.close()))
              .subscribe();
          } else {
            Browser.close();
          }
        }
      });
    });
  }
}
