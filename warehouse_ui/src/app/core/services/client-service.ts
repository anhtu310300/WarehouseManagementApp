import { inject, Service } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { API_Response } from '../models/interfaces/common.model';
import { GLOBAL_CONSTANT } from '../constant/global.constant';
import { ClientModel } from '../models/classes/client.model';

@Service()
export class ClientService {
  api_url: string = environment.API_URL;

  http = inject(HttpClient);

  getAllClient(): Observable<API_Response> {
  return this.http.get<API_Response>(
    this.api_url + '/' + GLOBAL_CONSTANT.API_METHODS.CUSTOMERS
  );
}

saveClient(obj: ClientModel): Observable<API_Response> {
  return this.http.post<API_Response>(
    this.api_url + '/' + GLOBAL_CONSTANT.API_METHODS.CUSTOMERS,
    obj
  );
}
}
