import { Component, inject, OnDestroy, OnInit, signal, WritableSignal } from '@angular/core';
import { Subscription } from 'rxjs';
import { EmployeeService } from '../../../core/services/employee-service';
import { IEmployeeResponse } from '../../../core/models/interfaces/employee.model';
import { IApiResponse } from '../../../core/models/interfaces/api-response.model';
import { HttpErrorResponse } from '@angular/common/http';
import { RouterLink } from '@angular/router';


@Component({
  imports: [RouterLink],
  selector: 'app-employee-list',
  styleUrl: './employee-list.css',
  templateUrl: './employee-list.html',
})
export class EmployeeList implements OnInit, OnDestroy {

employeeList: WritableSignal<IEmployeeResponse[]> = signal<IEmployeeResponse[]>([]);

subscription!: Subscription;


employeeService = inject(EmployeeService);

  ngOnInit(): void {
    this.getAllEmployee();
  }

  getAllEmployee() {
    this.subscription = this.employeeService.getAll().subscribe({
      next: (res: IApiResponse<IEmployeeResponse[]>) => {
        if (res.isSuccess) {
          this.employeeList.set(res.data ?? []);
        }
      },
      error: (err: HttpErrorResponse) => {
        alert(err.error.errorMessages);
      },
    });
  }
  
  
  onDeleteClient(id: number) {
    const isDelete = confirm('Are you sure you want to delete this customer?');
    
    if (!isDelete) {
      return;
    }
    
    this.subscription = this.employeeService.delete(id).subscribe({
      next: (res: IApiResponse<null>) => {
        if (res.isSuccess) {
          alert(res.message);
          this.getAllEmployee();
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
    if(this.subscription){
      this.subscription.unsubscribe();
    }
  }
}
