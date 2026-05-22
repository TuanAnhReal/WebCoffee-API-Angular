import { Routes } from '@angular/router';

import { authGuard } from './core/auth/guards/auth.guard';

import { LoginComponent } from './features/auth/pages/login/login.component';

import { AdminLayoutComponent } from './layouts/admin-layout/admin-layout.component';

export const routes: Routes = [

  {
    path: '',
    redirectTo: 'admin/dashboard',
    pathMatch: 'full'
  },

  {
    path: 'login',
    component: LoginComponent
  },

  {
    path: 'admin',

    canActivate: [authGuard],

    component: AdminLayoutComponent,

    children: [

      {
        path: 'dashboard',

        loadComponent: () =>
          import('./features/dashboard/pages/dashboard/dashboard.component')
            .then(c => c.DashboardPageComponent)
      }

    ]
  },

  {
    path: '**',
    redirectTo: 'login'
  }

];