import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { TagsPageRoutingModule } from './tags-routing.module';

import { TagsPage } from './tags.page';
import { AuthenticationButtonComponent } from '../shared/authentication-button/authentication-button.component';
import { AvatarComponentModule } from '../shared/avatar/avatar.module';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    AvatarComponentModule,
    AuthenticationButtonComponent,
    TagsPageRoutingModule
  ],
  declarations: [TagsPage]
})
export class TagsPageModule {}
