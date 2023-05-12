import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { RouteReuseStrategy } from '@angular/router';

import { IonicModule, IonicRouteStrategy } from '@ionic/angular';

import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';
import { CommonModule } from '@angular/common';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AuthInterceptorService } from './services/auth-interceptor.service';
import { JwtModule } from '@auth0/angular-jwt';
import { HttpErrorInterceptorService } from './services/http-error-interceptor.service';
import { CommonService } from './services/common.service';
import { RECAPTCHA_V3_SITE_KEY, RecaptchaV3Module } from "ng-recaptcha";
import { environment } from 'src/environments/environment';

//const redirectUri = 'com.iter-meum://dev-2mb38pu2.us.auth0.com/capacitor/com.iter-meum/callback';
export function tokenGetter(){
  return localStorage.getItem('jwt_token');
}

@NgModule({
  declarations: [AppComponent],
  imports: [
    BrowserModule, 
    IonicModule.forRoot(),
    AppRoutingModule, 
    CommonModule, 
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    RecaptchaV3Module,
    JwtModule.forRoot({
      config:{
        tokenGetter: tokenGetter,
        allowedDomains: ["https://localhost:7030", "http://localhost:8100", "https://iter-meum-api.azurewebsites.net"],
        disallowedRoutes:[]
      }
    })
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
      provide: RECAPTCHA_V3_SITE_KEY, 
      useValue: environment.recaptcha.siteKey
    }
  ]
})
export class AppModule {}
