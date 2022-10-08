import { Component, OnInit } from '@angular/core';
import { AuthService } from '@auth0/auth0-angular';

@Component({
  selector: 'app-login',
  templateUrl: './login.page.html',
  styleUrls: ['./login.page.scss'],
})
export class LoginPage implements OnInit {

  constructor(private auth: AuthService) { }

  ngOnInit() {
  }

  loginWithRedirect(): void {
    this.auth.loginWithRedirect({appState:{
      target:window.top.location.pathname
    }});
  }

}
