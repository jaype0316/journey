import { NgModule } from '@angular/core';
import { PreloadAllModules, RouterModule, Routes } from '@angular/router';
import { AuthGuard } from '@auth0/auth0-angular';
import { AuthenticatedGuard } from './guards/auth-guard.service';

const routes: Routes = [
  {
    path: '',
    loadChildren: () => import('./authenticate/authenticate.module').then( m => m.AuthenticatePageModule)
  },
  {
    path: 'tabs',
    loadChildren: () => import('./tabs/tabs.module').then(m => m.TabsPageModule),
    canActivate:[AuthenticatedGuard]
  },
  {
    path: 'new-chapter',
    loadChildren: () => import('./new-chapter/new-chapter.module').then( m => m.NewChapterPageModule),
    canActivate:[AuthenticatedGuard]
  },
  {
    path: 'update-chapter',
    loadChildren: () => import('./update-chapter/update-chapter.module').then( m => m.UpdateChapterPageModule),
    canActivate:[AuthenticatedGuard]
  },
  {
    path: 'authenticate',
    loadChildren: () => import('./authenticate/authenticate.module').then( m => m.AuthenticatePageModule)
  },
  {
    path: 'quote',
    loadChildren: () => import('./quote/quote.module').then( m => m.QuotePageModule),
    canActivate:[AuthenticatedGuard]
  },
  {
    path: 'about',
    loadChildren: () => import('./about/about.module').then( m => m.AboutPageModule),
    canActivate:[AuthenticatedGuard]
  },
  {
    path: 'register',
    loadChildren: () => import('./register/register.module').then( m => m.RegisterPageModule)
  },
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes, { preloadingStrategy: PreloadAllModules })
  ],
  exports: [RouterModule]
})
export class AppRoutingModule {}
