import { Component, inject, signal } from '@angular/core';
import { UserLoginModel } from '../../../core/models/classes/user.model';
import { FormsModule } from '@angular/forms';
import { User } from '../../../core/services/user';
import { IApiResponse } from '../../../core/models/interfaces/api-response.model';
import { IUser } from '../../../core/models/interfaces/IUser';
import { HttpErrorResponse } from '@angular/common/http';
import { Router } from '@angular/router';
import { GLOBAL_CONSTANT } from '../../../core/constant/global.constant';
import { CommonService } from '../../../core/services/common-service';

@Component({
  imports: [FormsModule],
  selector: 'app-login',
  styleUrl: './login.css',
  templateUrl: './login.html',
})
export class Login {

  loginObj: UserLoginModel = new UserLoginModel();

  userService = inject(User);
  route = inject(Router);
  commonService = inject(CommonService);

  isShowLoader = signal<boolean>(false);


  onLogin(){

    if (this.isShowLoader()) return;

    this.isShowLoader.set(true);
    this.commonService.isShowLoader$.next(true);

    this.userService.loginUser(this.loginObj).subscribe({
      next:(res: IApiResponse<IUser>) => {

        this.isShowLoader.set(false);
        this.commonService.isShowLoader$.next(false);

        if(res.isSuccess ==  true){
          localStorage.setItem(GLOBAL_CONSTANT.LOCAL_LOGIN_KEY, JSON.stringify(res.data));
          this.route.navigate(['admin/client-list']);
        }
      },
      error: (err: HttpErrorResponse) =>{

        this.isShowLoader.set(false);
        this.commonService.isShowLoader$.next(false);

         alert(err.error.errorMessages);
      }
    })
  }
}
