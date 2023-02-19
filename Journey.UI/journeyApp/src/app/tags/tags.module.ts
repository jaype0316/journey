import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { TagsPageRoutingModule } from './tags-routing.module';

import { TagsPage } from './tags.page';
import { AuthenticationButtonComponent } from '../shared/authentication-button/authentication-button.component';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    AuthenticationButtonComponent,
    TagsPageRoutingModule
  ],
  declarations: [TagsPage]
})
export class TagsPageModule {}
