import { Component, inject, signal } from '@angular/core';
import { IUser } from '../../../core/models/interfaces/IUser';
import { GLOBAL_CONSTANT } from '../../../core/constant/global.constant';
import { Router, RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { CommonService } from '../../../core/services/common-service';

@Component({
  imports: [RouterOutlet, RouterLink, RouterLinkActive],
  selector: 'app-layout',
  styleUrl: './layout.css',
  templateUrl: './layout.html',
})
export class Layout {

  loggedUser!: IUser;
  router = inject(Router);
  commonService = inject(CommonService);
  isShowLoader = signal<boolean>(false);

  constructor(){
    const localData = localStorage.getItem(GLOBAL_CONSTANT.LOCAL_LOGIN_KEY);
    if(localData != null) {
      this.loggedUser = JSON.parse(localData);
    }

    this.commonService.isShowLoader$.subscribe({
    next: (flag: boolean) => {
      this.isShowLoader.set(flag);
    },
  });
  }

  onSignOut(){
    localStorage.removeItem(GLOBAL_CONSTANT.LOCAL_LOGIN_KEY);
    this.router.navigateByUrl('/login');
  }
}
