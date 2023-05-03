import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { AuthenticationService } from '../services/auth.service';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastController } from '@ionic/angular';
import { NotificationService } from '../services/notification.service';
import { AuthService } from '@auth0/auth0-angular';
import { Browser } from '@capacitor/browser';

@Component({
  selector: 'app-authenticate',
  templateUrl: './authenticate.page.html',
  styleUrls: ['./authenticate.page.scss'],
})
export class AuthenticatePage implements OnInit {

  isBusy: boolean;
  errorMessages: [];
  fromRegistration:boolean = false;

  loginForm = new FormGroup({
    email:new FormControl('',[Validators.required, Validators.email]),
    password:new FormControl('',[ Validators.required, Validators.minLength(6)])
  });

  constructor(private http:HttpClient, private authService: AuthenticationService, 
              private router:Router, private toast: ToastController, private route: ActivatedRoute,
              private notifications: NotificationService, private externalAuth: AuthService) { }

  ngOnInit() {
    this.route.queryParams.subscribe(params => {
      console.log('query params = ', params);
      this.fromRegistration = params.fromRegistration;
    });
  }

  ionViewDidEnter(){
    this.externalAuth.isAuthenticated$.subscribe(isAuthenticated => {
      console.log('is authenticated from authenticate component == ', isAuthenticated)
      if(isAuthenticated){
        this.router.navigate(['/tabs/home']);
      }
    });
  }

  get email() {
    return this.loginForm.get('email');
  }
  get password(){
    return this.loginForm.get('password');
  }

  onClickLogin(){
    this.isBusy = true;

    if(!this.loginForm.valid)
      return;    
    
    this.authService.login(this.loginForm.value.email, this.loginForm.value.password).subscribe(success => {
      console.log('auth response == ', success);
      if(success){
        this.notifications.initPush();
        this.router.navigate(['/tabs/home']);
      }
      }, error => {
        console.log('error from authenticate componnet == ', error);
        this.showToast((<Array<string>>error.authFailures).join());
        this.isBusy = false;
      }, () => {
        this.isBusy = false;
      });
  }

  externalLogin(){
    // this.externalAuth.loginWithRedirect({
    //   async openUrl(url: string) {
    //     await Browser.open({ url, windowName: '_self' });
    //   }
    // })
    // .subscribe();
     this.externalAuth.loginWithRedirect();
  }

  externalLogout(){
    this.externalAuth.logout();
  }

  async showToast(message:string){
    const toast = await this.toast.create({
      message:message,
      duration: 2000,
      color:'danger'
    });
    toast.present();
  }

}
