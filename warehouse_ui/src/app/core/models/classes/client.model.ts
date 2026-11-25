import { ICreateCustomerRequest } from '../interfaces/customer.model';

export class ClientModel implements ICreateCustomerRequest {
  customerId = 0;
  custName = '';
  address = '';
  mobile = '';
  email = '';
  gstNo = '';
}
