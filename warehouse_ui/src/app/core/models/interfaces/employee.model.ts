export interface ICreateEmployeeRequest {
  empName: string;
  warehouseAssignedId: number | null;
  mobileNo: string;
  role: string;
  userName: string;
  password: string;
}

export interface IUpdateEmployeeRequest {
  empName: string;
  warehouseAssignedId: number | null;
  mobileNo: string;
  role: string;
  userName: string;
  password: string | null;
  isActive: boolean;
}

export interface IEmployeeResponse {
  empId: number;
  empName: string;
  warehouseAssignedId: number | null;
  mobileNo: string;
  role: string;
  userName: string;
  isActive: boolean;
}
