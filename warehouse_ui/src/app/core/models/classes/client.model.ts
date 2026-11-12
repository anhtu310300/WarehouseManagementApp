export class ClientModel {
  customerId: number;
  custName: string;
  address: string;
  mobile: string;
  email: string;
  gstNo: string;

  constructor() {
    this.customerId = 0;
    this.custName = '';
    this.address = '';
    this.mobile = '';
    this.email = '';
    this.gstNo = '';
  }
}
