import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { authGuard } from './shared/guard/auth.guard';

const routes: Routes = [
  { path: '', redirectTo: '/auth/login', pathMatch: 'full' },

  {
    path: 'auth',
    loadChildren: () => import('./components/auth/auth.module').then(m => m.AuthModule)
  },
  {
    canActivate : [authGuard],
    path: 'timeline',
    loadChildren: () => import('./components/timeline/timeline.module').then(m => m.TimelineModule)
  },
  {
    
    path: 'account',
    loadChildren: () => import('./components/account-setting/account-setting.module').then(m => m.AccountSettingModule)
  },
  { path: '**', redirectTo: 'auth/login' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
