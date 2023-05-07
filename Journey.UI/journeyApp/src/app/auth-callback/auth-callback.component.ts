import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '@auth0/auth0-angular';

@Component({
  selector: 'app-auth-callback',
  templateUrl: './auth-callback.component.html',
  styleUrls: ['./auth-callback.component.scss'],
})
export class AuthCallbackComponent implements OnInit {

  constructor(private auth: AuthService, private router:Router) { }

  ngOnInit() {
    if(!this.auth.isAuthenticated$){
      this.auth.loginWithRedirect().subscribe(resp => {
       this.router.navigate(['tabs/home']);
     });
    } else {
      this.router.navigate(['tabs/home']);
    }
  }
}
