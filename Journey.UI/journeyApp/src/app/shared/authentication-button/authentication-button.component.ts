import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { IonicModule } from '@ionic/angular';
import { AuthenticationService } from 'src/app/services/auth.service';

@Component({
  standalone:true,
  selector: 'app-authentication-button',
  imports:[IonicModule],
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
