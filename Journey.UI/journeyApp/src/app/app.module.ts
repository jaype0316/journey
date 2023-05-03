import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { RouteReuseStrategy } from '@angular/router';

import { IonicModule, IonicRouteStrategy, isPlatform } from '@ionic/angular';

import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';
import { CommonModule } from '@angular/common';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AuthInterceptorService } from './services/auth-interceptor.service';
import { JwtModule } from '@auth0/angular-jwt';
import { HttpErrorInterceptorService } from './services/http-error-interceptor.service';
import { CommonService } from './services/common.service';
import { SigninCallbackComponent } from './signin-callback/signin-callback.component';
import { SignoutCallbackComponent } from './signout-callback/signout-callback.component';
import { AuthModule } from '@auth0/auth0-angular';
import config from 'capacitor.config';
import { environment } from 'src/environments/environment';
import { AuthCallbackComponent } from './auth-callback/auth-callback.component';

//const redirectUri = 'com.iter-meum://dev-2mb38pu2.us.auth0.com/capacitor/com.iter-meum/callback';
export function tokenGetter(){
  return localStorage.getItem('jwt_token');
}

// Build the URL that Auth0 should redirect back to
export const authCallbackUri = isPlatform('capacitor') ? `${config.appId}://dev-2mb38pu2.us.auth0.com/capacitor/${config.appId}/${environment.clientRoot}auth-callback`
                                : environment.clientRoot;
console.log('redirect_uri== ', authCallbackUri);

@NgModule({
  declarations: [AppComponent,SigninCallbackComponent, SignoutCallbackComponent, AuthCallbackComponent],
  imports: [
    BrowserModule, 
    IonicModule.forRoot(),
    AppRoutingModule, 
    CommonModule, 
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    JwtModule.forRoot({
      config:{
        tokenGetter: tokenGetter,
        allowedDomains: ["https://localhost:7030", "http://localhost:8100", "https://iter-meum-api.azurewebsites.net"],
        disallowedRoutes:[]
      }
    }),
    AuthModule.forRoot({
      domain: "dev-2mb38pu2.us.auth0.com",
      clientId: "qIz5NRX97DPzPwobltrLgy8U6nZTDW9w",
      useRefreshTokens: true,
      useRefreshTokensFallback: false,
      authorizationParams: {
        authCallbackUri,
      }
    }),
  ],
  bootstrap: [AppComponent],
  providers: [
    CommonService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptorService,
      multi: true
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: HttpErrorInterceptorService,
      multi:true
    },
    {
      provide: RouteReuseStrategy, useClass: IonicRouteStrategy
    }
  ]
})
export class AppModule {}
