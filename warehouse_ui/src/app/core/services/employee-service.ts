import { Injectable } from '@angular/core';
import { GLOBAL_CONSTANT } from '../constant/global.constant';

import { GenericCrudService } from './generic-crud-service';
import { IEmployeeResponse } from '../models/interfaces/employee.model';

@Injectable({
  providedIn: 'root',
})
export class EmployeeService extends GenericCrudService<IEmployeeResponse> {
  constructor() {
    super(GLOBAL_CONSTANT.API_METHODS.EMPLOYEES);
  }
}
