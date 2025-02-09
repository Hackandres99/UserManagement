import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';

export const authGuard: CanActivateFn = (route, state) => {
  const token = localStorage.getItem('authToken');

  if (token) {
    // Redirigir si el usuario ya está autenticado
    const router = inject(Router);
    router.navigate(['/dashboard']);
    return false;
  }

  return true;
};

