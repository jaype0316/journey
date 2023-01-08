import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment';
import { Register } from '../models/register.model';

@Component({
  selector: 'app-register',
  templateUrl: './register.page.html',
  styleUrls: ['./register.page.scss'],
})
export class RegisterPage implements OnInit {

  registerForm = new FormGroup({
    email:new FormControl('',[Validators.required, Validators.email]),
    password:new FormControl('',[ Validators.required, Validators.minLength(6)]),
    confirmPassword: new FormControl('', [Validators.required, Validators.minLength(6)])
  }, this.mustMatch('password', 'confirmPassword'));

  constructor(private http:HttpClient, private router:Router) { 

  }

  ngOnInit() {
  
  }

  onSubmitRegistration(){
    if(!this.registerForm.valid)
      return;

      this.http.post(environment.journeyApi + 'Account/Register', this.registerForm.value).subscribe(() =>{
        this.router.navigate(['/authenticate']);
      });
    
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