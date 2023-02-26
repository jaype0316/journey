import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { IonicModule } from '@ionic/angular';

import { NewChapterPageRoutingModule } from './new-chapter-routing.module';

import { NewChapterPage } from './new-chapter.page';
import { AvatarComponentModule } from '../shared/avatar/avatar.module';


@NgModule({
  imports: [
    CommonModule,
    IonicModule,
    FormsModule,
    AvatarComponentModule,
    NewChapterPageRoutingModule
  ],
  declarations: [NewChapterPage]
})
export class NewChapterPageModule {}
