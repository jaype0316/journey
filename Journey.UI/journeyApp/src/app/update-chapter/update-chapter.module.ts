import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { UpdateChapterPageRoutingModule } from './update-chapter-routing.module';

import { UpdateChapterPage } from './update-chapter.page';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    UpdateChapterPageRoutingModule
  ],
  declarations: [UpdateChapterPage]
})
export class UpdateChapterPageModule {}
