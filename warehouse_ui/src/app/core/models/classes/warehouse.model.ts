import { IWarehouseResponse } from '../interfaces/warehouse.model';

export class WarehouseModel implements IWarehouseResponse {
  warehouseId = 0;
  title = '';
  address1 = '';
  address2 = '';
  city = '';
  totalArea = 0;
  isColdStorageAvailable: boolean | null = false;
}

