import { Routes } from '@angular/router';
import { authGuard } from './core/auth/guards/auth.guard';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'admin/dashboard',
    pathMatch: 'full'
  },
  {
    path: 'login',
    loadComponent: () =>
      import('./features/auth/pages/login/login.component')
        .then(c => c.LoginComponent)
  },
  {
    path: 'admin',
    canActivate: [authGuard],
    loadComponent: () =>
      import('./layouts/admin-layout/admin-layout.component')
        .then(c => c.AdminLayoutComponent),
    children: [
      // DASHBOARD
      {
        path: 'dashboard',
        loadChildren: () =>
          import('./features/dashboard/routes/dashboard.routes')
            .then(r => r.DASHBOARD_ROUTES)
      },

      // SẢN PHẨM
      {
        path: 'san-pham',
        loadChildren: () =>
          import('./features/san-pham/routes/san-pham.routes')
            .then(r => r.SAN_PHAM_ROUTES)
      },

      // KHÁCH HÀNG
      {
        path: 'khach-hang',
        loadChildren: () =>
          import('./features/khach-hang/khach-hang.routes')
            .then(r => r.KHACH_HANG_ROUTES)
      },

      // NHÂN VIÊN
      {
        path: 'nhan-vien',
        loadChildren: () =>
          import('./features/nhan-vien/routes/nhan-vien.routes')
            .then(r => r.NHAN_VIEN_ROUTES)
      },

      // HÓA ĐƠN
      {
        path: 'hoa-don',
        loadChildren: () =>
          import('./features/hoa-don/routes/hoa-don.routes')
            .then(r => r.HOA_DON_ROUTES)
      },
      // KHU VỰC - BÀN
      {
        path: 'khu-vuc-ban',
        loadChildren: () =>
          import('./features/khuvuc-ban/routes/khu-vuc-ban.routes')
            .then(r => r.KHU_VUC_BAN_ROUTES)
      },
      // DEFAULT ADMIN
      {
        path: '',
        redirectTo: 'dashboard',
        pathMatch: 'full'
      }
    ]
  },

  // FALLBACK (Bắt mọi đường dẫn sai)
  {
    path: '**',
    redirectTo: 'login'
  }
];