import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, NgModule, OnInit } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { IonicModule } from '@ionic/angular';
import { Subscription } from 'rxjs';
import { CommonService } from 'src/app/services/common.service';
import { environment } from 'src/environments/environment';


@Component({
  selector: 'app-avatar',
  templateUrl: './avatar.component.html',
  styleUrls: ['./avatar.component.scss'],
})

export class AvatarComponent implements OnInit {

  isBusy:boolean = false;
  subscriptions:Subscription[];
  profile:any;
  constructor(public router:Router, private http:HttpClient, private commonService:CommonService) { 
    this.subscriptions = new Array();
    this.commonService.hear().subscribe((result) => {
      this.profile.avatarUrl = result;
    })
  }

  ngOnInit() {
    this.isBusy = true;
    const profileRequest = this.http.get(environment.journeyApi + "Account/UserProfile").subscribe(up =>{
      this.profile = up;
      console.log('profile == ', this.profile);
    }, (err) => {}, () => { this.isBusy = false;});
    this.subscriptions.push(profileRequest);
  }

  ionViewDidEnter(){

  }

}
