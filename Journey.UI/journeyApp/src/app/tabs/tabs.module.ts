import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { TabsPageRoutingModule } from './tabs-routing.module';

import { TabsPage } from './tabs.page';
import { RouterModule, Routes } from '@angular/router';
import { AuthenticatedGuard } from '../guards/auth-guard.service';

const routes: Routes = [
  {
    path: '',
    component:TabsPage,
    children: [
      {
        path: 'home',
        loadChildren: () => import('../home/home.module').then(m => m.HomePageModule)
      },
      {
        path: 'chapters',
        loadChildren: () => import('../chapters/chapters.module').then(m => m.ChaptersPageModule),
        canActivate:[AuthenticatedGuard]
      },
      {
        path: 'chapters/new',
        loadChildren: () => import('../new-chapter/new-chapter.module').then(m => m.NewChapterPageModule),
        canActivate:[AuthenticatedGuard]
      },
      {
        path: 'chapter/update/:id',
        loadChildren: () => import('../update-chapter/update-chapter.module').then(m => m.UpdateChapterPageModule),
        canActivate:[AuthenticatedGuard]
      },
      {
        path: 'chapter/:id',
        loadChildren: () => import('../chapter/chapter.module').then(m => m.ChapterPageModule),
        canActivate:[AuthenticatedGuard]
      },
      {
        path: 'quote',
        loadChildren: () => import('../quote/quote.module').then(m => m.QuotePageModule)
      },
      {
        path: 'tags',
        loadChildren: () => import('../tags/tags.module').then(m => m.TagsPageModule),
        canActivate:[AuthenticatedGuard]
      },
      {
        path: 'about',
        loadChildren: () => import('../about/about.module').then(m => m.AboutPageModule)
      },
      {
        path: 'profile',
        loadChildren: () => import('../profile/profile.module').then(m => m.ProfilePageModule)
      },
      {
        path: '',
        redirectTo: '/tabs/home',
        pathMatch: 'full'
      }
    ]
  }
  // {
  //   path: '',
  //   redirectTo:'/tabs/home',
  //   pathMatch:'full'
  // }
];

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    TabsPageRoutingModule,
    RouterModule.forChild(routes)
  ],
  declarations: [TabsPage]
})
export class TabsPageModule {}
