import { CanActivateFn, Router } from '@angular/router';
import { GLOBAL_CONSTANT } from '../constant/global.constant';
import { inject } from '@angular/core';

export const authGuard: CanActivateFn = (route, state) => {
  const router = inject(Router);
  const isLocalDataPresent = localStorage.getItem(GLOBAL_CONSTANT.LOCAL_LOGIN_KEY);
  if (isLocalDataPresent != null) {
    return true;

  } else { 
    router.navigateByUrl("/login");
    return false;
  }
};
