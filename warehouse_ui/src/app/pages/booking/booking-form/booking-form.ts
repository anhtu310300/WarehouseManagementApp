import { Component, inject, signal } from '@angular/core';
import { AsyncPipe } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import {FormBuilder,FormGroup,ReactiveFormsModule,Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { catchError, map, Observable, of } from 'rxjs';

import { BookingService } from '../../../core/services/booking-service';
import { ClientService } from '../../../core/services/client-service';
import { WarehouseService } from '../../../core/services/warehouse-service';
import { SpaceService } from '../../../core/services/space-service';
import { EmployeeService } from '../../../core/services/employee-service';

import { BookingModel } from '../../../core/models/classes/booking.model';
import { WarehouseModel } from '../../../core/models/classes/warehouse.model';
import { SpaceModel } from '../../../core/models/classes/space.model';

import { IBookingResponse } from '../../../core/models/interfaces/booking.model';
import { ICustomerResponse } from '../../../core/models/interfaces/customer.model';
import { IEmployeeResponse } from '../../../core/models/interfaces/employee.model';
import { IApiResponse } from '../../../core/models/interfaces/api-response.model';
import { ISpaceAvailabilityResponse } from '../../../core/models/interfaces/space.model';

import { CommonService } from '../../../core/services/common-service';

@Component({
  imports: [ReactiveFormsModule, RouterLink, AsyncPipe],
  selector: 'app-booking-form',
  styleUrl: './booking-form.css',
  templateUrl: './booking-form.html',
})
export class BookingForm {
  commonService = inject(CommonService);
  isShowLoader = signal<boolean>(false);

  bookingId: number = 0;
  bookingForm!: FormGroup;

  customerList$!: Observable<IApiResponse<ICustomerResponse[]>>;
  warehouseList$!: Observable<IApiResponse<WarehouseModel[]>>;
  spaceList$: Observable<ISpaceAvailabilityResponse[]> = of<ISpaceAvailabilityResponse[]>([]);
  employeeList$!: Observable<IApiResponse<IEmployeeResponse[]>>;

  clientService = inject(ClientService);
  warehouseService = inject(WarehouseService);
  spaceService = inject(SpaceService);
  employeeService = inject(EmployeeService);

  formBuilder = inject(FormBuilder);
  bookingService = inject(BookingService);
  router = inject(Router);
  activatedRoute = inject(ActivatedRoute);

  constructor() {
    this.bookingId = Number(
      this.activatedRoute.snapshot.paramMap.get('bookingid'),
    );

    this.initilizeForm();

    if (this.bookingId > 0) {
      this.getBookingById();
    }

    this.getAllCustomer();
    this.getAllWarehouse();
    this.getAllSpace();
    this.getAllEmployee();
  }

  getAllCustomer(): void {
    this.customerList$ = this.clientService.getAll();
  }

  getAllWarehouse(): void {
    this.warehouseList$ = this.warehouseService.getAll();
  }

  getAllSpace(resetSpace: boolean = false): void {
    if (resetSpace) {
      this.bookingForm.patchValue({
        spaceId: 0,
      });
    }

    const warehouseId: number = this.bookingForm.value.warehouseId;
    const fromDate: string = this.bookingForm.value.bookingFromDate;
    const tillDate: string = this.bookingForm.value.bookingTillDate;
    const areaNeeded: number = Number(
      this.bookingForm.value.sqftAreaNeeded,
    );

    // Availability need warehouse, date, areaNeed to filter space
    if (!warehouseId || !fromDate || !tillDate || !areaNeeded) {
      this.spaceList$ = of<ISpaceAvailabilityResponse[]>([]);
      return;
    }

    this.spaceList$ = this.spaceService
      .getAvailability(warehouseId, fromDate, tillDate)
      .pipe(
        map(res =>
          (res.data ?? []).filter(
            space => space.availableAreaSqft >= areaNeeded,
          ),
        ),
        catchError((err: HttpErrorResponse) => {
          alert(
            err.error?.errorMessages?.join('\n') ||
              err.error?.message ||
              'Unable to get space availability.',
          );

          return of<ISpaceAvailabilityResponse[]>([]);
        }),
      );
  }

  getAllEmployee(): void {
    this.employeeList$ = this.employeeService.getAll();
  }

  initilizeForm(): void {
    this.bookingForm = this.formBuilder.group({
      customerId: [0, [Validators.required, Validators.min(1)]],

      warehouseId: [0, [Validators.required, Validators.min(1)]],

      spaceId: [0, [Validators.required, Validators.min(1)]],

      bookingFromDate: ['', [Validators.required]],

      bookingTillDate: ['', [Validators.required]],

      sqftAreaNeeded: [
        0,
        [
          Validators.required,
          Validators.min(1),
          Validators.pattern(/^\d+$/),
        ],
      ],

      totalAmount: [
        0,
        [Validators.required, Validators.min(0.01)],
      ],

      bookingDoneBy: [0, [Validators.required, Validators.min(1)]],
    });
  }

  getBookingById() {
    this.bookingService.getById(this.bookingId).subscribe({
      next: (res: IApiResponse<IBookingResponse>) => {
        if (res.isSuccess && res.data) {
          this.bookingForm.patchValue({
            ...res.data,
            bookingFromDate: res.data.bookingFromDate.slice(0, 10),
            bookingTillDate: res.data.bookingTillDate.slice(0, 10),
          });

          this.getAllSpace();
        } else {
          alert(res.errorMessages);
        }
      },
      error: (err: HttpErrorResponse) => {
        alert(err.error.errorMessages);
      },
    });
  }

  OnSaveBooking() {
    const formValue: BookingModel = this.bookingForm.value;

    if (formValue.bookingTillDate < formValue.bookingFromDate) {
    alert('End date must be equal or after start date');
    return;
  }
    if (this.bookingId > 0) {
      this.updateBooking(formValue);
    } else {
      this.createBooking(formValue);
    }
  }

  createBooking(formValue: BookingModel) {
    if (this.isShowLoader()) return;

    this.isShowLoader.set(true);
    this.commonService.isShowLoader$.next(true);

    this.bookingService.create(formValue).subscribe({
      next: (res: IApiResponse<IBookingResponse>) => {
        this.isShowLoader.set(false);
        this.commonService.isShowLoader$.next(false);

        if (res.isSuccess) {
          alert(res.message);
          this.router.navigateByUrl('/admin/bookings');
        }
      },
      error: (err: HttpErrorResponse) => {
        this.isShowLoader.set(false);
        this.commonService.isShowLoader$.next(false);

        alert(err.error.errorMessages);
      },
    });
  }

  updateBooking(formValue: BookingModel) {
    if (this.isShowLoader()) return;

    this.isShowLoader.set(true);
    this.commonService.isShowLoader$.next(true);

    this.bookingService.update(this.bookingId, formValue).subscribe({
      next: (res: IApiResponse<IBookingResponse>) => {
        this.isShowLoader.set(false);
        this.commonService.isShowLoader$.next(false);

        if (res.isSuccess) {
          alert(res.message);
          this.router.navigateByUrl('/admin/bookings');
        }
      },
      error: (err: HttpErrorResponse) => {
        this.isShowLoader.set(false);
        this.commonService.isShowLoader$.next(false);

        alert(err.error.errorMessages);
      },
    });
  }
}