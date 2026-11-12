import { Component, inject } from '@angular/core';
import { IUser } from '../../../core/models/interfaces/IUser';
import { GLOBAL_CONSTANT } from '../../../core/constant/global.constant';
import { Router, RouterLink, RouterOutlet } from '@angular/router';

@Component({
  imports: [RouterOutlet,RouterLink],
  selector: 'app-layout',
  styleUrl: './layout.css',
  templateUrl: './layout.html',
})
export class Layout {

  loggedUser!: IUser;
  router = inject(Router);

  constructor(){
    const localData = localStorage.getItem(GLOBAL_CONSTANT.LOCAL_LOGIN_KEY);
    if(localData != null) {
      this.loggedUser = JSON.parse(localData);
    }
  }

  onSignOut(){
    localStorage.removeItem(GLOBAL_CONSTANT.LOCAL_LOGIN_KEY);
    this.router.navigateByUrl('/login');
  }
}
