export class EmployeeModel {
  empId: number;
  empName: string;
  warehouseAssignedId: number | null;
  mobileNo: string;
  role: string;
  userName: string;
  isActive: boolean;
  password?: string;

  constructor() {
    this.empId = 0;
    this.empName = '';
    this.warehouseAssignedId = null;
    this.mobileNo = '';
    this.role = '';
    this.userName = '';
    this.isActive = true;
    this.password = '';
  }
}

