import { Component, inject } from '@angular/core';
import { UserLoginModel } from '../../../core/models/classes/user.model';
import { FormsModule } from '@angular/forms';
import { User } from '../../../core/services/user';
import { API_Response } from '../../../core/models/interfaces/common.model';
import { HttpErrorResponse } from '@angular/common/http';
import { Router } from '@angular/router';

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
      next:(res: API_Response) => {
        if(res.isSuccess ==  true){
          localStorage.setItem('warehouseUser', JSON.stringify(res.data));
          alert(res.message);
          this.route.navigate(['admin/client-list']);
        } 
      },
      error: (err: HttpErrorResponse) =>{
      const apiError = err.error as API_Response;

      const message = apiError?.errorMessages?.length
        ? apiError.errorMessages.join('\n')
        : 'Login failed. Please try again.';

         alert(message);
      }
    })
  }
}
