export interface ICreateBookingRequest {
  customerId: number;
  warehouseId: number;
  spaceId: number;
  bookingFromDate: string;
  bookingTillDate: string;
  sqftAreaNeeded: number;
  totalAmount: number;
  bookingDoneBy: number;
}

export interface IUpdateBookingRequest extends ICreateBookingRequest {}

export interface IBookingResponse {
  bookingId: number;
  customerId: number;
  warehouseId: number;
  spaceId: number | null;
  bookingFromDate: string;
  bookingTillDate: string;
  bookingDate: string;
  sqftAreaNeeded: number;
  totalAmount: number;
  bookingDoneBy: number;
}

export interface IBookingCalendarRequest {
  warehouseId: number;
  fromDate: string;
  tillDate: string;
}

export interface IBookingCalendarResponse {
  bookingId: number;
  customerId: number;
  customerName: string;
  warehouseId: number;
  spaceId: number | null;
  spaceName: string | null;
  bookingFromDate: string;
  bookingTillDate: string;
  sqftAreaNeeded: number;
  totalAmount: number;
}
