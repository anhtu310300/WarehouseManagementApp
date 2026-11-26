import { HttpErrorResponse } from '@angular/common/http';
import { AsyncPipe, DatePipe, DecimalPipe } from '@angular/common';
import { Component, ElementRef, inject, signal, ViewChild, WritableSignal } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Observable } from 'rxjs';

import { PaymentModel } from '../../../core/models/classes/payment.model';
import { IBookingResponse } from '../../../core/models/interfaces/booking.model';
import { ICreatePaymentRequest } from '../../../core/models/interfaces/payment.model';
import { IEmployeeResponse } from '../../../core/models/interfaces/employee.model';
import { IApiResponse } from '../../../core/models/interfaces/api-response.model';
import { PaymentService } from '../../../core/services/payment-service';
import { EmployeeService } from '../../../core/services/employee-service';

import { CommonService } from '../../../core/services/common-service';

@Component({
  imports: [ReactiveFormsModule, AsyncPipe, DatePipe, DecimalPipe],
  selector: 'app-payment-modal',
  styleUrl: './payment-modal.css',
  templateUrl: './payment-modal.html',
})
export class PaymentModal {
  commonService = inject(CommonService);
  isShowLoader = signal<boolean>(false);

  @ViewChild('paymentDialog') paymentDialog?: ElementRef<HTMLDialogElement>;

  selectedBooking: IBookingResponse | null = null;
  selectedPaymentId = 0;
  paymentList: WritableSignal<PaymentModel[]> = signal<PaymentModel[]>([]);
  paymentForm!: FormGroup;
  employeeList$!: Observable<IApiResponse<IEmployeeResponse[]>>;

  paymentService = inject(PaymentService);
  employeeService = inject(EmployeeService);
  formBuilder = inject(FormBuilder);

  constructor() {
    this.initializeForm();
    this.getAllEmployee();
  }

  initializeForm(): void {
    this.paymentForm = this.formBuilder.group({
      paymentAmount: [0, [Validators.required, Validators.min(0.01)]],
      paymentMode: ['', [Validators.required, Validators.maxLength(50)]],
      paymentNarration: ['', [Validators.required, Validators.maxLength(500)]],
      status: ['', [Validators.required, Validators.maxLength(50)]],
      receivedId: [0, [Validators.required, Validators.min(1)]],
    });
  }

  getAllEmployee(): void {
    this.employeeList$ = this.employeeService.getAll();
  }

  getAllPayment(booking?: IBookingResponse): void {
    if (booking) {
      this.selectedBooking = booking;
      this.paymentList.set([]);
      this.onClearForm();

      if (!this.paymentDialog?.nativeElement.open) {
        this.paymentDialog?.nativeElement.showModal();
      }
    }

    if (!this.selectedBooking) {
      return;
    }

    const bookingId = this.selectedBooking.bookingId;

    this.paymentService.getAll().subscribe({
      next: (res: IApiResponse<PaymentModel[]>) => {
        if (res.isSuccess && this.selectedBooking?.bookingId === bookingId) {
          this.paymentList.set(
            (res.data ?? []).filter(payment => payment.bookingId === bookingId),
          );
        }
      },
      error: (err: HttpErrorResponse) => {
        alert(err.error.errorMessages);
      },
    });
  }

  getPaymentById(): void {
    const id = this.selectedPaymentId;

    this.paymentService.getById(id).subscribe({
      next: (res: IApiResponse<PaymentModel>) => {
        if (res.isSuccess && res.data && this.selectedPaymentId === id) {
          this.paymentForm.patchValue(res.data);
        }
      },
      error: (err: HttpErrorResponse) => {
        alert(err.error.errorMessages);
      },
    });
  }

  OnSavePayment(): void {
    const formValue: ICreatePaymentRequest = this.paymentForm.value;
    formValue.bookingId = this.selectedBooking!.bookingId;

    if (this.selectedPaymentId > 0) {
      this.updatePayment(formValue);
    } else {
      this.createPayment(formValue);
    }
  }

  onEditPayment(id: number): void {
    this.selectedPaymentId = id;
    this.getPaymentById();
  }

  createPayment(formValue: ICreatePaymentRequest): void {
    if (this.isShowLoader()) return;

    this.isShowLoader.set(true);
    this.commonService.isShowLoader$.next(true);

    this.paymentService.create(formValue).subscribe({
      next: (res: IApiResponse<PaymentModel>) => {
        this.isShowLoader.set(false);
        this.commonService.isShowLoader$.next(false);

        if (res.isSuccess) {
          alert(res.message);
          this.onClearForm();
          this.getAllPayment();
        }
      },
      error: (err: HttpErrorResponse) => {
        this.isShowLoader.set(false);
        this.commonService.isShowLoader$.next(false);

        alert(err.error.errorMessages);
      },
    });
  }

  updatePayment(formValue: ICreatePaymentRequest, id = this.selectedPaymentId): void {
    if (this.isShowLoader()) return;

    this.isShowLoader.set(true);
    this.commonService.isShowLoader$.next(true);

    this.paymentService.update(id, formValue).subscribe({
      next: (res: IApiResponse<PaymentModel>) => {
        this.isShowLoader.set(false);
        this.commonService.isShowLoader$.next(false);

        if (res.isSuccess) {
          alert(res.message);

          if (this.selectedPaymentId === id) {
            this.onClearForm();
          }

          this.getAllPayment();
        }
      },
      error: (err: HttpErrorResponse) => {
        this.isShowLoader.set(false);
        this.commonService.isShowLoader$.next(false);

        alert(err.error.errorMessages);
      },
    });
  }

  onDeletePayment(id: number): void {
    const isDelete = confirm('Are you sure you want to delete this payment?');

    if (!isDelete) {
      return;
    }

    this.paymentService.delete(id).subscribe({
      next: (res: IApiResponse<null>) => {
        if (res.isSuccess) {
          alert(res.message);

          if (this.selectedPaymentId === id) {
            this.onClearForm();
          }

          this.getAllPayment();
        }
      },
      error: (err: HttpErrorResponse) => {
        alert(err.error.errorMessages);
      },
    });
  }

  onClearForm(): void {
    this.selectedPaymentId = 0;
    this.paymentForm.reset({
      paymentAmount: 0,
      paymentMode: '',
      paymentNarration: '',
      status: '',
      receivedId: 0,
    });
  }
}
