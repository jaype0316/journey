import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { RouteReuseStrategy } from '@angular/router';

import { IonicModule, IonicRouteStrategy } from '@ionic/angular';

import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';
import { CommonModule } from '@angular/common';
import { AuthModule, AuthHttpInterceptor } from '@auth0/auth0-angular';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';

@NgModule({
  declarations: [AppComponent],
  imports: [
    BrowserModule, 
    IonicModule.forRoot(),
    AppRoutingModule, 
    CommonModule, 
    HttpClientModule,
    AuthModule.forRoot({
      domain:'dev-2mb38pu2.us.auth0.com',
      clientId:'XaJDsh1K9YUMMBTDE0UFtQLYAj86v6nN',
      redirectUri:'http://localhost:8101/tabs/home', //window.location.origin
      audience:"https://beyourhero.journey.com/api",
      //scope:'read:current_user',
      httpInterceptor:{
        allowedList:[
          {
             // Match any request that starts 'https://YOUR_DOMAIN/api/v2/' (note the asterisk)
             uri: 'https://localhost:7030/api/*',
            //  tokenOptions: {
            //   // The attached token should target this audience
            //   audience: 'https://beyourhero.journey.com/api',   
            //   // The attached token should have these scopes
            //   scope: 'read:current_user'
            // }
          }
        ]
      }
  })],
  // providers: [{ 
  //   provide: RouteReuseStrategy, 
  //   useClass: IonicRouteStrategy 
  // },{
  //   provide:HTTP_INTERCEPTORS,
  //   useClass:AuthHttpInterceptor,
  //   multi:true
  // }],
  providers: [{
    provide:HTTP_INTERCEPTORS,
    useClass:AuthHttpInterceptor,
    multi:true
  }],
  bootstrap: [AppComponent],
})
export class AppModule {}
