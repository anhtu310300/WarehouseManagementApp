import { IBookingResponse } from '../interfaces/booking.model';

export class BookingModel implements IBookingResponse {
  bookingId = 0;
  customerId = 0;
  warehouseId = 0;
  spaceId: number | null = null;
  bookingFromDate = '';
  bookingTillDate = '';
  bookingDate = '';
  sqftAreaNeeded = 0;
  totalAmount = 0;
  bookingDoneBy = 0;
}
