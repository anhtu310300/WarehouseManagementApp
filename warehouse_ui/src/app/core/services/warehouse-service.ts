import { Injectable } from '@angular/core';
import { GLOBAL_CONSTANT } from '../constant/global.constant';
import { WarehouseModel } from '../models/classes/warehouse.model';
import { GenericCrudService } from './generic-crud-service';

@Injectable({
  providedIn: 'root',
})
export class WarehouseService extends GenericCrudService<WarehouseModel> {
  constructor() {
    super(GLOBAL_CONSTANT.API_METHODS.WAREHOUSES);
  }
}
