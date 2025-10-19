import { CanActivateFn, Router } from '@angular/router';

export const authGuard: CanActivateFn = (route, state) => {
  const token = localStorage.getItem('pos_jwt_token');
  if (token) return true;
  const router = new Router();
  router.navigate(['/auth/login']);
  return false;
};
