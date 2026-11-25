export interface IWarehouseSpaceDashboard {
  spaceId: number;
  spaceName: string;
  totalAreaSqft: number;
  bookedAreaSqft: number;
  availableAreaSqft: number;
  activeBookings: number;
  isAvailable: boolean;
}

export interface IWarehouseDashboardResponse {
  warehouseId: number;
  warehouseName: string;
  dashboardDate: string;
  warehouseAreaSqft: number;
  totalSpaces: number;
  totalSpaceAreaSqft: number;
  bookedAreaSqft: number;
  availableAreaSqft: number;
  activeBookings: number;
  spaces: IWarehouseSpaceDashboard[];
}

export interface IOwnerDashboardResponse {
  totalWarehouses: number;
  totalSpaces: number;
  activeSpaces: number;
  inactiveSpaces: number;
  totalCustomers: number;
  totalEmployees: number;
  activeEmployees: number;
  totalBookings: number;
  totalPayments: number;
  completedPayments: number;
  totalBookingAmount: number;
  totalPaidAmount: number;
  remainingAmount: number;
}
