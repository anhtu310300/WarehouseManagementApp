import { Component, inject, OnDestroy, OnInit, signal, WritableSignal } from '@angular/core';
import { ClientService } from '../../../core/services/client-service';
import { API_Response } from '../../../core/models/interfaces/common.model';
import { HttpErrorResponse } from '@angular/common/http';
import { ClientModel } from '../../../core/models/classes/client.model';
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

  clientList: WritableSignal<ClientModel[]> = signal<ClientModel[]>([]); 
  clientService = inject (ClientService);

  subScription!: Subscription;
  isCardView: boolean = false;

  ngOnInit(): void {
    this.getAllClient();
  }

  getAllClient(){
    this.subScription = this.clientService.getAllClient().subscribe({
      next: (res:API_Response) =>{
         if (res.isSuccess) {
        this.clientList.set(res.data);
      }
      },
      error: (err:HttpErrorResponse) =>{
        console.error(err);
      }
    })
  }

  ngOnDestroy(): void{
    this.subScription.unsubscribe();
  }
}
