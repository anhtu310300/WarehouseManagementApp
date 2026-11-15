import { Component, inject, OnDestroy, OnInit, signal, WritableSignal } from '@angular/core';
import { Subscription } from 'rxjs';
import { EmployeeService } from '../../../core/services/employee-service';
import { EmployeeModel } from '../../../core/models/classes/employee.model';
import { API_Response } from '../../../core/models/interfaces/common.model';
import { HttpErrorResponse } from '@angular/common/http';
import { RouterLink } from '@angular/router';
import { WarehouseModel } from '../../../core/models/classes/warehouse.model';
import { WarehouseService } from '../../../core/services/warehouse-service';

@Component({
  imports: [RouterLink],
  selector: 'app-employee-list',
  styleUrl: './employee-list.css',
  templateUrl: './employee-list.html',
})
export class EmployeeList implements OnInit, OnDestroy {

employeeList: WritableSignal<EmployeeModel[]> = signal<EmployeeModel[]>([]);
warehouseList = signal<WarehouseModel[]>([]);
warehouseService = inject(WarehouseService);

subscription!: Subscription;


employeeService = inject(EmployeeService);

  ngOnInit(): void {
    this.getAllEmployee();
    this.getAllWarehouse();
  }

  getAllEmployee() {
    this.subscription = this.employeeService.getAll().subscribe({
      next: (res: API_Response) => {
        if (res.isSuccess) {
          this.employeeList.set(res.data);
        }
      },
      error: (err: HttpErrorResponse) => {
        console.error(err);
      },
    });
  }
  
  getAllWarehouse(): void {
  this.warehouseService.getAll().subscribe({
    next: (res: API_Response<WarehouseModel[]>) => {
      if (res.isSuccess) {
        this.warehouseList.set(res.data ?? []);
      }
    }
  });
}
  
  onDeleteClient(id: number) {
    const isDelete = confirm('Are you sure you want to delete this customer?');
    
    if (!isDelete) {
      return;
    }
    
    this.subscription = this.employeeService.delete(id).subscribe({
      next: (res: API_Response) => {
        if (res.isSuccess) {
          alert(res.message);
          this.getAllEmployee();
        } else {
          alert(res.errorMessages);
        }
      },
      error: (err: HttpErrorResponse) => {
        alert(err.message);
      }
    });
  }
  
  
  ngOnDestroy(): void {
    if(this.subscription){
      this.subscription.unsubscribe();
    }
  }
}
