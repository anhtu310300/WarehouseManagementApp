import { Component, inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { GLOBAL_CONSTANT } from '../../../core/constant/global.constant';
import { IUser } from '../../../core/models/interfaces/IUser';
import { ClientService } from '../../../core/services/client-service';
import { API_Response } from '../../../core/models/interfaces/common.model';
import { HttpErrorResponse } from '@angular/common/http';
import { Router } from '@angular/router';

@Component({
  imports: [ReactiveFormsModule],
  selector: 'app-client-form',
  styleUrl: './client-form.css',
  templateUrl: './client-form.html',
})
export class ClientForm {
  loggedUser!: IUser;
  clientForm!: FormGroup;

  formBuilder = inject(FormBuilder);
  clientService = inject(ClientService);
  router = inject(Router);

  constructor() {
    const localUserData = localStorage.getItem(GLOBAL_CONSTANT.LOCAL_LOGIN_KEY);
    if (localUserData) {
      this.loggedUser = JSON.parse(localUserData);
    }

    this.initializeForm();
  }

  initializeForm() {
    this.clientForm = this.formBuilder.group({
      custName: ['', [Validators.required, Validators.maxLength(50)]],

      address: ['', [Validators.maxLength(50)]],

      mobile: ['', [Validators.required, Validators.maxLength(50)]],

      email: ['', [Validators.email, Validators.maxLength(50)]],

      gstNo: ['', [Validators.maxLength(50)]],
    });
  }

  OnSaveClient() {
    if (this.clientForm.invalid) {
      this.clientForm.markAllAsTouched();
      return;
    }

    const formValue = this.clientForm.value;
    this.clientService.saveClient(formValue).subscribe({
      next: (res: API_Response) => {
        if (res.isSuccess) {
          alert(res.message);
          this.router.navigateByUrl('/admin/client-list');
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
