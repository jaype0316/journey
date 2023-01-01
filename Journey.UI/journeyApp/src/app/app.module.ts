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
    JwtModule.forRoot({
      config:{
        tokenGetter: tokenGetter,
        allowedDomains: ["https://localhost:7030", "http://localhost:8100"],
        disallowedRoutes:[]
      }
    })
  //   AuthModule.forRoot({
  //     domain:'dev-2mb38pu2.us.auth0.com',
  //     clientId: 'XaJDsh1K9YUMMBTDE0UFtQLYAj86v6nN',//'XaJDsh1K9YUMMBTDE0UFtQLYAj86v6nN',
  //     redirectUri: redirectUri, //'http://localhost:8101/tabs/home', //window.location.origin
  //     audience:"https://beyourhero.journey.com/api",
  //     //scope:'read:current_user',
  //     httpInterceptor:{
  //       allowedList:[
  //         {
  //            // Match any request that starts 'https://YOUR_DOMAIN/api/v2/' (note the asterisk)
  //            uri: 'https://localhost:7030/api/*',
  //           //  tokenOptions: {
  //           //   // The attached token should target this audience
  //           //   audience: 'https://beyourhero.journey.com/api',   
  //           //   // The attached token should have these scopes
  //           //   scope: 'read:current_user'
  //           // }
  //         },
  //         {
  //           uri: 'http://iter-meum.com/api/*'
  //         }
  //       ]
  //     }
  // })],
  ],
  bootstrap: [AppComponent],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptorService,
      multi: true
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: HttpErrorInterceptorService,
      multi:true
    }
  ]
})
export class AppModule {}
