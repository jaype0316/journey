import { NgModule } from '@angular/core';
import { PreloadAllModules, RouterModule, Routes } from '@angular/router';
import { AuthGuard } from '@auth0/auth0-angular';

const routes: Routes = [
  // {
  //   path: '',
  //   loadChildren: () => import('./tabs/tabs.module').then(m => m.TabsPageModule),
  //   //canActivate:[AuthGuard]
  // },
  {
    path: '',
    loadChildren: () => import('./authenticate/authenticate.module').then( m => m.AuthenticatePageModule)
    //canActivate:[AuthGuard]
  },
  {
    path: 'new-chapter',
    loadChildren: () => import('./new-chapter/new-chapter.module').then( m => m.NewChapterPageModule),
    //canActivate:[AuthGuard]
  },
  {
    path: 'update-chapter',
    loadChildren: () => import('./update-chapter/update-chapter.module').then( m => m.UpdateChapterPageModule),
    //canActivate:[AuthGuard]
  },
  {
    path: 'authenticate',
    loadChildren: () => import('./authenticate/authenticate.module').then( m => m.AuthenticatePageModule)
  },
  {
    path: 'quote',
    loadChildren: () => import('./quote/quote.module').then( m => m.QuotePageModule)
  },
  {
    path: 'about',
    loadChildren: () => import('./about/about.module').then( m => m.AboutPageModule)
  },
  {
    path: 'authenticate',
    loadChildren: () => import('./authenticate/authenticate.module').then( m => m.AuthenticatePageModule)
  },
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes, { preloadingStrategy: PreloadAllModules })
  ],
  exports: [RouterModule]
})
export class AppRoutingModule {}
