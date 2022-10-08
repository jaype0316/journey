import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { TabsPage } from './tabs.page';

const routes: Routes = [
  {
    path: 'tabs',
    component:TabsPage,
    children: [
      {
        path: 'home',
        loadChildren: () => import('../home/home.module').then(m => m.HomePageModule)
      },
      {
        path: 'chapters',
        loadChildren: () => import('../chapters/chapters.module').then(m => m.ChaptersPageModule)
      },
      {
        path: 'chapters/new',
        loadChildren: () => import('../new-chapter/new-chapter.module').then(m => m.NewChapterPageModule)
      },
      {
        path: 'chapter/update/:id',
        loadChildren: () => import('../update-chapter/update-chapter.module').then(m => m.UpdateChapterPageModule)
      },
      {
        path: 'chapter/:id',
        loadChildren: () => import('../chapter/chapter.module').then(m => m.ChapterPageModule)
      },
      {
        path: 'quote',
        loadChildren: () => import('../quote/quote.module').then(m => m.QuotePageModule)
      },
      {
        path: 'profile',
        loadChildren: () => import('../profile/profile.module').then(m => m.ProfilePageModule)
      },
      {
        path: 'about',
        loadChildren: () => import('../about/about.module').then(m => m.AboutPageModule)
      },
      {
        path: 'callback',
        loadChildren: () => import('../home/home.module').then(m => m.HomePageModule)
      },
      {
        path: '',
        redirectTo: '/tabs/home',
        pathMatch: 'full'
      }
    ]
  },
  {
    path: '',
    redirectTo:'/tabs/home',
    pathMatch:'full'
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class TabsPageRoutingModule {}
