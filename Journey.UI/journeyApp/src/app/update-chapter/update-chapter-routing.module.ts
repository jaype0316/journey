import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { UpdateChapterPage } from './update-chapter.page';

const routes: Routes = [
  {
    path: '',
    component: UpdateChapterPage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class UpdateChapterPageRoutingModule {}
