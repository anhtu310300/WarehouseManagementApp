import { Component, inject, signal } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { GLOBAL_CONSTANT } from '../../../core/constant/global.constant';
import { IUser } from '../../../core/models/interfaces/IUser';
import { ClientService } from '../../../core/services/client-service';
import { IApiResponse } from '../../../core/models/interfaces/api-response.model';
import { HttpErrorResponse } from '@angular/common/http';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { ClientModel } from '../../../core/models/classes/client.model';
import { ICustomerResponse } from '../../../core/models/interfaces/customer.model';

import { CommonService } from '../../../core/services/common-service';

@Component({
  imports: [ReactiveFormsModule,RouterLink],
  selector: 'app-client-form',
  styleUrl: './client-form.css',
  templateUrl: './client-form.html',
})
export class ClientForm {
  commonService = inject(CommonService);
  isShowLoader = signal<boolean>(false);

  loggedUser!: IUser;
  clientForm!: FormGroup;
  clientId: number = 0; // UI divides into 2 create/ update mode depends on this value

  formBuilder = inject(FormBuilder);
  clientService = inject(ClientService);
  router = inject(Router);
  activatedRoute = inject(ActivatedRoute);

  constructor() {
    // Take user information in localstorage
    const localUserData = localStorage.getItem(GLOBAL_CONSTANT.LOCAL_LOGIN_KEY);
    if (localUserData) {
      this.loggedUser = JSON.parse(localUserData);
    }
    // Create empty form
    this.initializeForm();

    // Read id from url
    this.clientId = Number(this.activatedRoute.snapshot.paramMap.get('id'));

    // if id exist -> edit mode , if edit mode, we need to know which one to edit (id)
    // otherwise id = 0 like initilize above means create mode
    if (this.clientId > 0) {
      this.getClientById();
    }
  }

  initializeForm(): void {
    this.clientForm = this.formBuilder.group({
      custName: ['', [Validators.required, Validators.maxLength(50)]],

      address: ['', [Validators.required, Validators.maxLength(50)]],

      mobile: ['', [Validators.required, Validators.maxLength(50)]],

      email: ['', [Validators.required, Validators.email, Validators.maxLength(50)]],

      gstNo: ['', [Validators.required, Validators.maxLength(50)]],
    });
  }

  getClientById() {
    this.clientService.getById(this.clientId).subscribe({
      next: (res: IApiResponse<ICustomerResponse>) => {
        if (res.isSuccess && res.data) {
          this.clientForm.patchValue(res.data);
        }
      },
      error: (err: HttpErrorResponse) => {
        alert(err.error.errorMessages);
      },
    });
  }

  OnSaveClient() {

    const formValue: ClientModel = this.clientForm.value;

    if (this.clientId > 0) {
      this.updateClient(formValue);
    } else {
      this.createClient(formValue);
    }
  }

  createClient(formValue: ClientModel) {
    if (this.isShowLoader()) return;

    this.isShowLoader.set(true);
    this.commonService.isShowLoader$.next(true);

    this.clientService.create(formValue).subscribe({
      next: (res: IApiResponse<ICustomerResponse>) => {
        this.isShowLoader.set(false);
        this.commonService.isShowLoader$.next(false);

        if (res.isSuccess) {
          alert(res.message);
          this.router.navigateByUrl('/admin/client-list');
        }
      },
      error: (err: HttpErrorResponse) => {
        this.isShowLoader.set(false);
        this.commonService.isShowLoader$.next(false);

        alert(err.error.errorMessages);
      },
    });
  }

  updateClient(formValue: ClientModel) {
    if (this.isShowLoader()) return;

    this.isShowLoader.set(true);
    this.commonService.isShowLoader$.next(true);

    this.clientService.update(this.clientId, formValue).subscribe({
      next: (res: IApiResponse<ICustomerResponse>) => {
        this.isShowLoader.set(false);
        this.commonService.isShowLoader$.next(false);

        if (res.isSuccess) {
          alert(res.message);
          this.router.navigateByUrl('/admin/client-list');
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
