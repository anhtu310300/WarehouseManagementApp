import { Component, inject, OnDestroy, OnInit, signal, ViewChild, WritableSignal } from '@angular/core';
import { DatePipe, DecimalPipe } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { RouterLink } from '@angular/router';
import { Subscription } from 'rxjs';

import { PaymentModal } from '../payment-modal/payment-modal';
import { BookingService } from '../../../core/services/booking-service';
import { IBookingResponse } from '../../../core/models/interfaces/booking.model';
import { IApiResponse } from '../../../core/models/interfaces/api-response.model';

@Component({
  imports: [RouterLink, DatePipe, DecimalPipe, PaymentModal],
  selector: 'app-booking-list',
  styleUrl: './booking-list.css',
  templateUrl: './booking-list.html',
})
export class BookingList implements OnInit, OnDestroy {
  @ViewChild('paymentModal') paymentModal?: PaymentModal;

  bookingList: WritableSignal<IBookingResponse[]> = signal<IBookingResponse[]>([]);

  subscription!: Subscription;

  bookingService = inject(BookingService);

  ngOnInit(): void {
    this.getAllBooking();
  }

  getAllBooking() {
    this.subscription = this.bookingService.getAll().subscribe({
      next: (res: IApiResponse<IBookingResponse[]>) => {
        if (res.isSuccess) {
          this.bookingList.set(res.data ?? []);
        }
      },
      error: (err: HttpErrorResponse) => {
        alert(err.error.errorMessages);
      },
    });
  }

  openPaymentModal(booking: IBookingResponse): void {
    this.paymentModal?.getAllPayment(booking);
  }

  onDeleteBooking(id: number) {
    const isDelete = confirm('Are you sure you want to delete this booking?');

    if (!isDelete) {
      return;
    }

    this.subscription = this.bookingService.delete(id).subscribe({
      next: (res: IApiResponse<null>) => {
        if (res.isSuccess) {
          alert(res.message);
          this.getAllBooking();
        } else {
          alert(res.errorMessages);
        }
      },
      error: (err: HttpErrorResponse) => {
        alert(err.error.errorMessages);
      },
    });
  }

  ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }
}
