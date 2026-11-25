import { Component, inject, OnDestroy, OnInit, signal, WritableSignal } from '@angular/core';
import { ClientService } from '../../../core/services/client-service';
import { IApiResponse } from '../../../core/models/interfaces/api-response.model';
import { HttpErrorResponse } from '@angular/common/http';
import { ICustomerResponse } from '../../../core/models/interfaces/customer.model';
import { Subscription } from 'rxjs';
import { NgClass } from '@angular/common';
import { RouterLink } from '@angular/router';

@Component({
  imports: [NgClass, RouterLink],
  selector: 'app-client-list',
  styleUrl: './client-list.css',
  templateUrl: './client-list.html',
})
export class ClientList implements OnInit, OnDestroy{

  clientList: WritableSignal<ICustomerResponse[]> = signal<ICustomerResponse[]>([]); 
  clientService = inject (ClientService);

  subScription!: Subscription;
  isCardView: boolean = false;

  ngOnInit(): void {
    this.getAllClient();
  }

  getAllClient(){
    this.subScription = this.clientService.getAll().subscribe({
      next: (res:IApiResponse<ICustomerResponse[]>) =>{
         if (res.isSuccess) {
        this.clientList.set(res.data ?? []);
      }
      },
      error: (err:HttpErrorResponse) =>{
        alert(err.error.errorMessages);
      }
    })
  }

  onDeleteClient(id: number) {
    const isDelete = confirm('Are you sure you want to delete this customer?');

    if (!isDelete) {
      return;
    }

    this.subScription = this.clientService.delete(id).subscribe({
      next: (res: IApiResponse<null>) => {
        if (res.isSuccess) {
          alert(res.message);
          this.getAllClient();
        }
      },
      error: (err: HttpErrorResponse) => {
        alert(err.error.errorMessages);
      }
    });
  }

  ngOnDestroy(): void{
    if (this.subScription) {
      this.subScription.unsubscribe();
    }
  }
}
