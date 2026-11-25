import { IEmployeeResponse } from '../interfaces/employee.model';

export class EmployeeModel implements IEmployeeResponse {
  empId = 0;
  empName = '';
  warehouseAssignedId: number | null = null;
  mobileNo = '';
  role = '';
  userName = '';
  isActive = true;
  password = '';
}
