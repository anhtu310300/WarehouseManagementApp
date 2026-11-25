import { Component, inject } from '@angular/core';
import { UserLoginModel } from '../../../core/models/classes/user.model';
import { FormsModule } from '@angular/forms';
import { User } from '../../../core/services/user';
import { IApiResponse } from '../../../core/models/interfaces/api-response.model';
import { IUser } from '../../../core/models/interfaces/IUser';
import { HttpErrorResponse } from '@angular/common/http';
import { Router } from '@angular/router';
import { GLOBAL_CONSTANT } from '../../../core/constant/global.constant';

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
  
  onLogin(){
    this.userService.loginUser(this.loginObj).subscribe({
      next:(res: IApiResponse<IUser>) => {
        if(res.isSuccess ==  true){
          localStorage.setItem(GLOBAL_CONSTANT.LOCAL_LOGIN_KEY, JSON.stringify(res.data));
          alert(res.message);
          this.route.navigate(['admin/client-list']);
        } 
      },
      error: (err: HttpErrorResponse) =>{
      const apiError = err.error as IApiResponse;

      const message = apiError?.errorMessages?.length
        ? apiError.errorMessages.join('\n')
        : 'Login failed. Please try again.';

         alert(message);
      }
    })
  }
}
