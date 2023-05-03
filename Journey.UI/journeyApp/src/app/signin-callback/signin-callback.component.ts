import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '@auth0/auth0-angular';

@Component({
  selector: 'app-signin-callback',
  templateUrl: './signin-callback.component.html',
  styleUrls: ['./signin-callback.component.scss'],
})
export class SigninCallbackComponent implements OnInit {

  constructor(private authService:AuthService, private router:Router) { }

  ngOnInit() {

    this.router.navigate(['/'], { replaceUrl:true});
  }

}
