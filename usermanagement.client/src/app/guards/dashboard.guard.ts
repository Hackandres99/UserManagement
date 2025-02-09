import { CanActivateFn, Router } from '@angular/router';
import { inject } from '@angular/core';

export const dashboardGuard: CanActivateFn = (route, state) => {
  const router = inject(Router);
  const token = localStorage.getItem('authToken');

  if (!token) {
    router.navigate([''])
    return false
  }

  return true
};
