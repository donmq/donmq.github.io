import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

// Import Containers
import { AdminLayoutComponent, DefaultLayoutComponent } from './containers';

import { AdminGuard } from './_core/_gurad/admin.guard';
import { UserGuard } from './_core/_gurad/user.guard';


export const routes: Routes = [
  {
    path: '',
    redirectTo: 'home',
    pathMatch: 'full',
  },
  {
    path: '',
    component: DefaultLayoutComponent,
    data: {
      title: 'Home'
    },
    children: [
      {
        path: '',
        loadChildren: () => import('./views/main/main.module').then(m => m.MainModule)
      }
    ]
  },
  {
    path: 'admin',
    component: AdminLayoutComponent,
    data: {
      title: 'Home'
    },
    canActivate: [UserGuard],
    canActivateChild: [AdminGuard],
    children: [
      {
        path: '',
        loadChildren: () => import('./views/admin/admin.module').then(m => m.AdminModule)
      },
    ]
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { relativeLinkResolution: 'legacy' })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
