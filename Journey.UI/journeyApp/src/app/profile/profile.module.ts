import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { ProfilePageRoutingModule } from './profile-routing.module';

import { ProfilePage } from './profile.page';
import { AuthenticationButtonComponent } from '../shared/authentication-button/authentication-button.component';
import { AvatarComponentModule } from '../shared/avatar/avatar.module';


@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    ProfilePageRoutingModule,
    AuthenticationButtonComponent,
    AvatarComponentModule
  ],
  declarations: [ProfilePage]
})
export class ProfilePageModule {}
