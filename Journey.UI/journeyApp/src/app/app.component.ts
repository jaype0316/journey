import { Component, NgZone } from '@angular/core';
import { mergeMap } from 'rxjs/operators';
import { Browser } from '@capacitor/browser';
import { App } from '@capacitor/app';
import { AuthenticationService } from './services/auth.service';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '@auth0/auth0-angular';
import { environment } from 'src/environments/environment';
import { isPlatform } from '@ionic/angular';
import config from 'capacitor.config';


// Build the URL that Auth0 should redirect back to
export const authCallbackUri = isPlatform('capacitor') ? `${config.appId}://dev-2mb38pu2.us.auth0.com/capacitor/${config.appId}/callback`
                                : environment.clientRoot;

@Component({
  selector: 'app-root',
  templateUrl: 'app.component.html',
  styleUrls: ['app.component.scss'],
})
export class AppComponent {
  public appPages = [
    { title: 'Profile', url: '/tabs/profile', icon: 'person' },
    { title: 'About', url: '/tabs/about', icon: 'reader' }
    //{ title: 'Logout', url: '', icon: 'exit', onClick: this.clickLogout }
  ];
  public labels = [{title: 'About', url: '/tabs/about', icon: 'finger-print'}];

  constructor(private ngZone:NgZone, public auth: AuthenticationService, private router:Router, 
    private externalAuth: AuthService, private route:ActivatedRoute) {}

  clickLogout(){
    this.externalAuth.logout();
  }

  ngOnInit():void {
     // Use Capacitor's App plugin to subscribe to the `appUrlOpen` event
     this.externalAuth.isAuthenticated$.subscribe(isAuthenticated => {
        console.log('is authenticated == ', isAuthenticated);
        if(isAuthenticated){
          this.externalAuth.getAccessTokenSilently().subscribe(token => {
            localStorage.setItem('accessToken', token);
            console.log('token obtained!!');
          })
        }
     });

    if(!isPlatform('capacitor')){
      App.addListener('appUrlOpen', ({ url }) => {
        // Must run inside an NgZone for Angular to pick up the changes
        // https://capacitorjs.com/docs/guides/angular
        this.ngZone.run(() => {
          if (url?.startsWith(authCallbackUri)) {
            // If the URL is an authentication callback URL..
            if (
              url.includes('state=') &&
              (url.includes('error=') || url.includes('code='))
            ) {
              // Call handleRedirectCallback and close the browser
              this.externalAuth.handleRedirectCallback(url).pipe(mergeMap(() => Browser.close())).subscribe();
            } else {
              Browser.close();
            }
          }
        });
      });
     }  
  }
}
