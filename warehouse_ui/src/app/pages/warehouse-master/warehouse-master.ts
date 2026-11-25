import { Component, computed, inject, OnDestroy, OnInit, signal, ViewChild, WritableSignal, } from '@angular/core';
import { WarehouseModel } from '../../core/models/classes/warehouse.model';
import { WarehouseService } from '../../core/services/warehouse-service';
import { Subscription } from 'rxjs';

import { HttpErrorResponse } from '@angular/common/http';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { SpaceModal } from './space-modal/space-modal';
import { IApiResponse } from '../../core/models/interfaces/api-response.model';

@Component({
  imports: [ReactiveFormsModule, SpaceModal],
  selector: 'app-warehouse-master',
  styleUrl: './warehouse-master.css',
  templateUrl: './warehouse-master.html',
})
export class WarehouseMaster implements OnInit, OnDestroy {
  @ViewChild('spaceModal') spaceModal?: SpaceModal;

  selectedWarehouseId: number = 0; // 0 -> add mode ; has warehouseId -> edit mode
  warehouseList: WritableSignal<WarehouseModel[]> = signal<WarehouseModel[]>([]);
  warehouseForm!: FormGroup;
  warehouseService = inject(WarehouseService);

  formBuilder = inject(FormBuilder);

  subScription!: Subscription;

  ngOnInit(): void {
    this.initializeForm();
    this.getAllWarehouse();
  }

  getAllWarehouse() {
    this.subScription = this.warehouseService.getAll().subscribe({
      next: (res) => {
        if (res.isSuccess && res.data) {
          this.warehouseList.set(res.data);
        }
      },
      error: (err: HttpErrorResponse) => {
        alert(err.error.errorMessages);
      },
    });
  }

  getWarehouseById() {
    this.warehouseService.getById(this.selectedWarehouseId).subscribe({
      next: (res) => {
        if (res.isSuccess && res.data) {
          this.warehouseForm.patchValue(res.data);
        }
      },
      error: (err: HttpErrorResponse) => {
        alert(err.error.errorMessages);
      },
    });
  }

  initializeForm(): void {
    this.warehouseForm = this.formBuilder.group({
      title: ['', [Validators.required, Validators.maxLength(50)]],

      address1: ['', [Validators.required, Validators.maxLength(500)]],

      address2: ['', [Validators.required, Validators.maxLength(1000)]],

      city: ['', [Validators.required, Validators.maxLength(50)]],

      totalArea: [0, [Validators.required, Validators.min(1)]],

      isColdStorageAvailable: [false],
    });
  }

  OnSaveWarehouse() {


    const formValue: WarehouseModel = this.warehouseForm.value;

    if (this.selectedWarehouseId > 0) {
      this.updateWarehouse(formValue);
    } else {
      this.createWarehouse(formValue);
    }
  }

  onEditWarehouse(id: number): void {
    this.selectedWarehouseId = id;
    this.getWarehouseById();
  } // we need because we don't pass id through route

openSpaceModal(warehouse: WarehouseModel): void {
  this.spaceModal?.getAllSpace(warehouse);
}


  createWarehouse(formValue: WarehouseModel) {
    this.warehouseService.create(formValue).subscribe({
      next: (res: IApiResponse) => {
        if (res.isSuccess) {
          alert(res.message);
          this.getAllWarehouse();
          this.onClearForm();
        }
      },
      error: (err: HttpErrorResponse) => {
        alert(err.error.errorMessages);
      },
    });
  }

  updateWarehouse(formValue: WarehouseModel) {
    this.warehouseService.update(this.selectedWarehouseId, formValue).subscribe({
      next: (res: IApiResponse) => {
        if (res.isSuccess) {
          alert(res.message);
          this.getAllWarehouse();
          this.onClearForm();
        }
      },
      error: (err: HttpErrorResponse) => {
        alert(err.error.errorMessages);
      },
    });
  }

  onDeleteWarehouse(id: number) {
    const isDelete = confirm('Are you sure you want to delete this warehouse?');

    if (!isDelete) {
      return;
    }

    this.subScription = this.warehouseService.delete(id).subscribe({
      next: (res: IApiResponse) => {
        if (res.isSuccess) {
          alert(res.message);
          this.getAllWarehouse();
        }
      },
      error: (err: HttpErrorResponse) => {
        alert(err.error.errorMessages);
      },
    });
  }

  onClearForm(): void {
    this.selectedWarehouseId = 0;

    this.warehouseForm.reset({
      title: '',
      address1: '',
      address2: '',
      city: '',
      totalArea: 0,
      isColdStorageAvailable: false,
    });
  }

  totalStorageArea = computed(() =>
    this.warehouseList().reduce((total, warehouse) => total + warehouse.totalArea, 0),
  );

  coldStorageEnabledCount = computed(
    () =>
      this.warehouseList().filter((warehouse) => warehouse.isColdStorageAvailable === true).length,
  );

  totalCities = computed(
    () =>
      new Set(
        this.warehouseList()
          .map((warehouse) => warehouse.city.trim().toLowerCase())
          .filter((city) => city.length > 0),
      ).size,
  );




  ngOnDestroy(): void {
    if (this.subScription) {
      this.subScription.unsubscribe();
    }
  }
}
