import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { GLOBAL_CONSTANT } from '../constant/global.constant';
import { BookingModel } from '../models/classes/booking.model';
import { IBookingCalendarResponse } from '../models/interfaces/booking.model';
import { IApiResponse } from '../models/interfaces/api-response.model';
import { GenericCrudService } from './generic-crud-service';

@Injectable({
  providedIn: 'root',
})
export class BookingService extends GenericCrudService<BookingModel> {
  constructor() {
    super(GLOBAL_CONSTANT.API_METHODS.BOOKINGS);
  }

  getCalendar(
    warehouseId: number,
    fromDate: string,
    tillDate: string,
  ): Observable<IApiResponse<IBookingCalendarResponse[]>> {
    return this.http.get<IApiResponse<IBookingCalendarResponse[]>>(
      `${this.resourceUrl}/calendar`,
      {
        params: {
          warehouseId,
          fromDate,
          tillDate,
        },
      },
    );
  }
}