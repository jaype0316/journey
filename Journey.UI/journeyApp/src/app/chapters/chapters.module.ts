import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';
import { ChaptersPageRoutingModule } from './chapters-routing.module';

import { ChaptersPage } from './chapters.page';
import { AuthenticationButtonComponent } from '../shared/authentication-button/authentication-button.component';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    ChaptersPageRoutingModule,
    AuthenticationButtonComponent
  ],
  declarations: [ChaptersPage]
})
export class ChaptersPageModule {}
