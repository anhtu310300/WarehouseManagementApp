import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { UserLoginModel } from '../models/classes/user.model';
import { environment } from '../../../environments/environment';
import { GLOBAL_CONSTANT } from '../constant/global.constant';
import { Observable } from 'rxjs';
import { IApiResponse } from '../models/interfaces/api-response.model';
import { IUser } from '../models/interfaces/IUser';

@Injectable({
    providedIn: 'root'
})
export class User {

    http = inject(HttpClient);
    api_url: string = environment.API_URL;

    loginUser(obj: UserLoginModel): Observable<IApiResponse<IUser>>{
        return this.http.post<IApiResponse<IUser>>(`${this.api_url}/${GLOBAL_CONSTANT.API_METHODS.LOGIN}`, obj)
    }
}
