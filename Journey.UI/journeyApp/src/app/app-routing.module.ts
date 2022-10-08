import { NgModule } from '@angular/core';
import { PreloadAllModules, RouterModule, Routes } from '@angular/router';
import { AuthGuard } from '@auth0/auth0-angular';

const routes: Routes = [
  {
    path: '',
    loadChildren: () => import('./tabs/tabs.module').then(m => m.TabsPageModule),
    canActivate:[AuthGuard]
  },
  {
    path: 'new-chapter',
    loadChildren: () => import('./new-chapter/new-chapter.module').then( m => m.NewChapterPageModule),
    canActivate:[AuthGuard]
  },
  {
    path: 'update-chapter',
    loadChildren: () => import('./update-chapter/update-chapter.module').then( m => m.UpdateChapterPageModule),
    canActivate:[AuthGuard]
  },
  {
    path: 'login',
    loadChildren: () => import('./login/login.module').then( m => m.LoginPageModule)
  },
  {
    path: 'quote',
    loadChildren: () => import('./quote/quote.module').then( m => m.QuotePageModule)
  },
  {
    path: 'about',
    loadChildren: () => import('./about/about.module').then( m => m.AboutPageModule)
  },
  // {
  //   path: 'folder/:id',
  //   loadChildren: () => import('./folder/folder.module').then( m => m.FolderPageModule)
  // },
  // {
  //   path: 'home',
  //   component: HomeComponent
  //   //loadChildren: () => import('./home/home.module').then( m => m.HomePageModule)
  // },
  // {
  //   path: 'chapters',
  //   component: ChaptersComponent
  //   //loadChildren: () => import('./chapters/chapters.module').then( m => m.ChaptersPageModule)
  // },
  // {
  //   path: 'tags',
  //   component: TagsComponent
  //   //loadChildren: () => import('./tags/tags.module').then( m => m.TagsPageModule)
  // },
  // {
  //   path: 'chapters',
  //   loadChildren: () => import('./chapters/chapters.module').then( m => m.ChaptersPageModule)
  // },
  // {
  //   path: 'tags',
  //   loadChildren: () => import('./tags/tags.module').then( m => m.TagsPageModule)
  // },
  // {
  //   path: 'home',
  //   loadChildren: () => import('./home/home.module').then( m => m.HomePageModule)
  // },
  // {
  //   path: 'discover',
  //   loadChildren: () => import('./discover/discover.module').then( m => m.DiscoverPageModule)
  // },
  // {
  //   path: 'tabs',
  //   loadChildren: () => import('./tabs/tabs.module').then( m => m.TabsPageModule)
  // }
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes, { preloadingStrategy: PreloadAllModules })
  ],
  exports: [RouterModule]
})
export class AppRoutingModule {}
