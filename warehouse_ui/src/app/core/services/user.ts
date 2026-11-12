import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { UserLoginModel } from '../models/classes/user.model';
import { environment } from '../../../environments/environment';
import { GLOBAL_CONSTANT } from '../constant/global.constant';
import { Observable } from 'rxjs';
import { API_Response } from '../models/interfaces/common.model';

@Injectable({
    providedIn: 'root'
})
export class User {

    http = inject(HttpClient);
    api_url: string = environment.API_URL;

    loginUser(obj: UserLoginModel): Observable<API_Response>{
        return this.http.post<API_Response>(`${this.api_url}/${GLOBAL_CONSTANT.API_METHODS.LOGIN}`, obj)
    }
}
