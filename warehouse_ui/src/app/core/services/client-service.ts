import { Injectable } from '@angular/core';
import { GLOBAL_CONSTANT } from '../constant/global.constant';
import { ICustomerResponse } from '../models/interfaces/customer.model';
import { GenericCrudService } from './generic-crud-service';

@Injectable({
  providedIn: 'root',
})
export class ClientService extends GenericCrudService<ICustomerResponse> {
  constructor() {
    super(GLOBAL_CONSTANT.API_METHODS.CUSTOMERS);
  }
}
