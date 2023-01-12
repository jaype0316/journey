import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { ChapterPageRoutingModule } from './chapter-routing.module';

import { ChapterPage } from './chapter.page';
import {SwiperModule} from 'swiper/angular'


import SwiperCore, { Autoplay, Keyboard, Pagination, Navigation, Scrollbar, Virtual, Zoom } from 'swiper';
import { IonicSlides } from '@ionic/angular';
import { AuthenticationButtonComponent } from '../shared/authentication-button/authentication-button.component';

SwiperCore.use([Autoplay, Keyboard, Pagination, Scrollbar, Zoom, Virtual, Navigation, IonicSlides]);

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    ChapterPageRoutingModule,
    SwiperModule,
    AuthenticationButtonComponent
  ],
  declarations: [ChapterPage]
})
export class ChapterPageModule {}
