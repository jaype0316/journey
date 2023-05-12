import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment';
import { Register } from '../models/register.model';
import { ReCaptchaV3Service } from 'ng-recaptcha';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-register',
  templateUrl: './register.page.html',
  styleUrls: ['./register.page.scss'],
})
export class RegisterPage implements OnInit {

  errors:[];
  isBusy:boolean;
  SITE_KEY: '';
  isCaptchaValid: boolean = false;
  subscriptions:Subscription[];

  registerForm = new FormGroup({
    email:new FormControl('',[Validators.required, Validators.email]),
    password:new FormControl('',[ Validators.required, Validators.minLength(6)]),
    confirmPassword: new FormControl('', [Validators.required, Validators.minLength(6)]),
    captchaToken: new FormControl('')
  }, this.mustMatch('password', 'confirmPassword'));

  constructor(private http:HttpClient, private router:Router, private recaptchaService: ReCaptchaV3Service) { 
    this.subscriptions = new Array();
  }

  ngOnInit() {
  
  }

  ionViewWillLeave(){
    this.registerForm.reset();
    this.errors = [];
    this.subscriptions.forEach(s => s.unsubscribe());
  }

  onSubmitRegistration(){
    this.errors = [];

    if(!this.registerForm.valid)
      return;

      let captchaPromise = this.recaptchaService.execute('register')
                          .subscribe((token) => this.handleCaptchaToken(token));  

      this.subscriptions.push(captchaPromise);  
  }

  private handleCaptchaToken(token){
    if(!token)
      return;
    //https://www.google.com/recaptcha/api/siteverify METHOD: POST
   

    this.isBusy = true; 
    this.registerForm.get('captchaToken').setValue(token);
    console.log('register form == ', this.registerForm.getRawValue());
    let registerPromise = this.http.post(environment.journeyApi + 'Account/Register', this.registerForm.value).subscribe(() =>{
      this.isBusy = false;
      this.router.navigate(['/authenticate'],
                          { queryParams: { fromRegistration: true }});
    }, errorResponse => {
        this.errors = errorResponse.errors;
        this.isBusy = false;
        console.log('errors == ', this.errors);
    });  

    this.subscriptions.push(registerPromise);
  }

  get email() {
    return this.registerForm.get('email');
  }
  get password(){
    return this.registerForm.get('password');
  }
  get confirmPassword(){
    return this.registerForm.get('confirmPassword');
  }

  get siteKey(){
    return environment.recaptcha.siteKey;
  }

  private mustMatch(controlName: string, matchingControlName: string) {
    return (formGroup: FormGroup) => {
      const control = formGroup.controls[controlName];
      const matchingControl = formGroup.controls[matchingControlName];

      if (matchingControl.errors && !matchingControl.errors.mustMatch) {
        return;
      }

      // set error on matchingControl if validation fails
      if (control.value !== matchingControl.value) {
        matchingControl.setErrors({ mustMatch: true });
      } else {
        matchingControl.setErrors(null);
      }
      return null;
    };
  }

}
