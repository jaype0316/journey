import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { UpdateChapterPageRoutingModule } from './update-chapter-routing.module';

import { UpdateChapterPage } from './update-chapter.page';
import { AvatarComponentModule } from '../shared/avatar/avatar.module';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    AvatarComponentModule,
    UpdateChapterPageRoutingModule
  ],
  declarations: [UpdateChapterPage]
})
export class UpdateChapterPageModule {}
