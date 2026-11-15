export class WarehouseModel {
  warehouseId: number;
  title: string;
  address1: string;
  address2: string;
  city: string;
  totalArea: number;
  isColdStorageAvailable: boolean | null;

  constructor() {
    this.warehouseId = 0;
    this.title = '';
    this.address1 = '';
    this.address2 = '';
    this.city = '';
    this.totalArea = 0;
    this.isColdStorageAvailable = false;
  }
}

