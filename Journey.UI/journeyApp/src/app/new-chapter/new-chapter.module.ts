import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { NewChapterPageRoutingModule } from './new-chapter-routing.module';

import { NewChapterPage } from './new-chapter.page';

@NgModule({
  imports: [
    CommonModule,
    IonicModule,
    FormsModule,
    NewChapterPageRoutingModule
  ],
  declarations: [NewChapterPage]
})
export class NewChapterPageModule {}
