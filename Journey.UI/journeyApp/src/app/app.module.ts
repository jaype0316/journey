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
import { authInfo } from './authinfo';

//const redirectUri = 'com.iter-meum://dev-2mb38pu2.us.auth0.com/capacitor/com.iter-meum/callback';
export function tokenGetter(){
  return localStorage.getItem('jwt_token');
}

// Build the URL that Auth0 should redirect back to
console.log('redirect_uri== ', authInfo.authCallback);

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
      domain: authInfo.domain,
      clientId: authInfo.clientId,
      useRefreshTokens: authInfo.useRefreshTokens,
      useRefreshTokensFallback: authInfo.useRefreshTokensFallback,
      authorizationParams: {
        authCallbackUri: authInfo.authCallback,
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
