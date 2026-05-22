import {
  inject
} from '@angular/core';

import {
  CanActivateFn,
  Router
} from '@angular/router';

import {
  AuthService
} from '../services/auth.service';

export const roleGuard: CanActivateFn = (
  route
) => {

  const authService =
    inject(AuthService);

  const router =
    inject(Router);

  const requiredRole =
    route.data?.['role'];

  if (
    requiredRole &&
    authService.hasRole(requiredRole)
  ) {

    return true;
  }

  router.navigate(['/403']);

  return false;
};