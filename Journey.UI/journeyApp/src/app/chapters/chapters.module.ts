import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';
import { ChaptersPageRoutingModule } from './chapters-routing.module';

import { ChaptersPage } from './chapters.page';
import { AuthenticationButtonComponent } from '../shared/authentication-button/authentication-button.component';
import { AvatarComponentModule } from '../shared/avatar/avatar.module';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    AvatarComponentModule,
    ChaptersPageRoutingModule,
    AuthenticationButtonComponent
  ],
  declarations: [ChaptersPage]
})
export class ChaptersPageModule {}
