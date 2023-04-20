import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { AboutPageRoutingModule } from './about-routing.module';

import { AboutPage } from './about.page';
import { AuthenticationButtonComponent } from '../shared/authentication-button/authentication-button.component';
import { AvatarComponentModule } from '../shared/avatar/avatar.module';


@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    AboutPageRoutingModule,
    AvatarComponentModule,
    AuthenticationButtonComponent
  ],
  declarations: [AboutPage]
})
export class AboutPageModule {}
