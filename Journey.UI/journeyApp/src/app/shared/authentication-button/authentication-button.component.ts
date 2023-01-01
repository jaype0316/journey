import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthenticationService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-authentication-button',
  templateUrl: './authentication-button.component.html',
  styleUrls: ['./authentication-button.component.scss'],
})
export class AuthenticationButtonComponent implements OnInit {

  constructor(public auth:AuthenticationService, private router: Router) { }

  ngOnInit() {}

  clickLogout(): void {
    this.auth.logout().subscribe(c =>{
      this.router.navigate(['authenticate']);
    });
    // this.auth.logout({
    //   returnTo:'http://localhost:8101/'
    // });
  }

}
