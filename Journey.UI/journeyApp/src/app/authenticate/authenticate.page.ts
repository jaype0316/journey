import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-authenticate',
  templateUrl: './authenticate.page.html',
  styleUrls: ['./authenticate.page.scss'],
})
export class AuthenticatePage implements OnInit {

  welcome:{ login:{}, register:{}}
  constructor() { }

  ngOnInit() {
    this.welcome = { login:{}, register:{}};
  }

}
