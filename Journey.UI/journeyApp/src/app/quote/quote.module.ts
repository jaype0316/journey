import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { QuotePageRoutingModule } from './quote-routing.module';

import { QuotePage } from './quote.page';
import { AuthenticationButtonComponent } from '../shared/authentication-button/authentication-button.component';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    QuotePageRoutingModule
  ],
  declarations: [QuotePage, AuthenticationButtonComponent]
})
export class QuotePageModule {}
