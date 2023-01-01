import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { AuthenticationService } from '../services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-authenticate',
  templateUrl: './authenticate.page.html',
  styleUrls: ['./authenticate.page.scss'],
})
export class AuthenticatePage implements OnInit {

  isBusy: boolean;

  loginForm = new FormGroup({
    email:new FormControl('',[Validators.required, Validators.email]),
    password:new FormControl('',[ Validators.required, Validators.minLength(6)])
  });

  constructor(private http:HttpClient, private authService: AuthenticationService, private router:Router) { }

  ngOnInit() {
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
    
    this.authService.login(this.loginForm.value.email, this.loginForm.value.password).subscribe(authResponse => {
      const tokenResponse = (<any>authResponse);
      if(tokenResponse && tokenResponse.token)
        this.router.navigate(['/tabs/home']);
    },err =>{}, () => {this.isBusy = false;});
  }

}
