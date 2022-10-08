import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { NewChapterPage } from './new-chapter.page';

const routes: Routes = [
  {
    path: '',
    component: NewChapterPage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class NewChapterPageRoutingModule {}
