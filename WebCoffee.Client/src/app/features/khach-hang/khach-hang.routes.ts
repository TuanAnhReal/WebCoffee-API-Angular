import { Routes } from '@angular/router';

export const KHACH_HANG_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () => import('./pages/khach-hang-list/khach-hang-list.component').then(m => m.KhachHangListComponent)
  }
];