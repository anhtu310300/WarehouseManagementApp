export interface ICreateWarehouseRequest {
  title: string;
  address1: string;
  address2: string;
  city: string;
  totalArea: number;
  isColdStorageAvailable: boolean | null;
}

export interface IUpdateWarehouseRequest extends ICreateWarehouseRequest {}

export interface IWarehouseResponse extends ICreateWarehouseRequest {
  warehouseId: number;
}
