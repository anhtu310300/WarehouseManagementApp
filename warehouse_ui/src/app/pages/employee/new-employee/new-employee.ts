import { Component, inject, signal } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { EmployeeService } from '../../../core/services/employee-service';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { EmployeeModel } from '../../../core/models/classes/employee.model';
import { API_Response } from '../../../core/models/interfaces/common.model';
import { HttpErrorResponse } from '@angular/common/http';
import { WarehouseModel } from '../../../core/models/classes/warehouse.model';
import { WarehouseService } from '../../../core/services/warehouse-service';

@Component({
  imports: [ReactiveFormsModule, RouterLink],
  selector: 'app-new-employee',
  styleUrl: './new-employee.css',
  templateUrl: './new-employee.html',
})
export class NewEmployee {
  employeeId: number = 0;
  employeeForm!: FormGroup;

  warehouseList = signal<WarehouseModel[]>([]);
  warehouseService = inject(WarehouseService);

  formBuilder = inject(FormBuilder);
  employeeService = inject(EmployeeService);
  router = inject(Router);
  activatedRoute = inject(ActivatedRoute);

  constructor() {
    this.employeeId = Number(this.activatedRoute.snapshot.paramMap.get('empid'));

    this.initilizeForm();

    if (this.employeeId > 0) {
      this.getEmployeeById();
    }
    this.getAllWarehouse();
  }

  getAllWarehouse(): void {
    this.warehouseService.getAll().subscribe({
      next: (res: API_Response<WarehouseModel[]>) => {
        if (res.isSuccess) {
          this.warehouseList.set(res.data ?? []);
        }
      },
    });
  }

  initilizeForm(): void {
    this.employeeForm = this.formBuilder.group({
      empName: ['', [Validators.required, Validators.maxLength(50)]],

      warehouseAssignedId: [null as number | null, [Validators.min(1)]],

      mobileNo: ['', [Validators.required, Validators.pattern(/^\d{10}$/)]],

      role: ['', [Validators.required, Validators.maxLength(50)]],

      userName: ['', [Validators.required, Validators.maxLength(50)]],

      password: [
        '',
        this.employeeId > 0
          ? [Validators.minLength(8), Validators.maxLength(100)]
          : [Validators.required, Validators.minLength(8), Validators.maxLength(100)],
      ], // if employeeId = null which is false, no validation needed as we keep old password
    });

    if (this.employeeId > 0) {
      this.employeeForm.addControl('isActive', this.formBuilder.control(true));
      // addControll is used to add 1 formControl into FormGroup after the form was created, 
      // in this situation, it only be executed if the condition is statisfied
    }
  }

  getEmployeeById() {
    this.employeeService.getById(this.employeeId).subscribe({
      next: (res: API_Response) => {
        if (res.isSuccess) {
          this.employeeForm.patchValue(res.data);
        } else {
          alert(res.errorMessages);
        }
      },
      error: (err: HttpErrorResponse) => {
        alert(err.message);
      },
    });
  }

  OnSaveEmployee() {
    if (this.employeeForm.invalid) {
      this.employeeForm.markAllAsTouched();
      return;
    }

    const formValue: EmployeeModel = this.employeeForm.value;

    if (this.employeeId > 0) {
      this.updateEmployee(formValue);
    } else {
      this.createEmployee(formValue);
    }
  }

  createEmployee(formValue: EmployeeModel) {
    this.employeeService.create(formValue).subscribe({
      next: (res: API_Response) => {
        if (res.isSuccess) {
          alert(res.message);
          this.router.navigateByUrl('/admin/employees');
        } else {
          alert(res.errorMessages);
        }
      },

      error: (err: HttpErrorResponse) => {
        alert(err.message);
      },
    });
  }

  updateEmployee(formValue: EmployeeModel) {
    this.employeeService.update(this.employeeId, formValue).subscribe({
      next: (res: API_Response) => {
        if (res.isSuccess) {
          alert(res.message);
          this.router.navigateByUrl('/admin/employees');
        } else {
          alert(res.errorMessages);
        }
      },
      error: (err: HttpErrorResponse) => {
        alert(err.message);
      },
    });
  }
}
// //"empName": "string",
//   "warehouseAssignedId": 0,
//   "mobileNo": "string",
//   "role": "string",
//   "userName": "string",
//   "password": "string"
