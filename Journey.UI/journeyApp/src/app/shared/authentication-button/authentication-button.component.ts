import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthModule, AuthService } from '@auth0/auth0-angular';
import { IonicModule } from '@ionic/angular';
import { AuthenticationService } from 'src/app/services/auth.service';

@Component({
  standalone:true,
  selector: 'app-authentication-button',
  imports:[IonicModule, CommonModule, AuthModule],
  templateUrl: './authentication-button.component.html',
  styleUrls: ['./authentication-button.component.scss'],
})
export class AuthenticationButtonComponent implements OnInit {

  constructor(public auth:AuthenticationService, private router: Router, private externalAuth:AuthService) { }

  ngOnInit() {}

  clickLogout(): void {

    this.auth.logout();

    // this.auth.logout().subscribe(c =>{
    //   this.router.navigate(['authenticate']);
    // });
    // this.auth.logout({
    //   returnTo:'http://localhost:8101/'
    // });
  }

  get isAuthenticated() {
    return this.auth?.isAuthenticated();
  }

}
