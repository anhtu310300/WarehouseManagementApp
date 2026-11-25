export interface ICreateCustomerRequest {
  custName: string;
  address: string;
  mobile: string;
  email: string;
  gstNo: string;
}

export interface IUpdateCustomerRequest extends ICreateCustomerRequest {}

export interface ICustomerResponse {
  customerId: number;
  custName: string | null;
  address: string;
  mobile: string;
  email: string;
  gstNo: string;
}
