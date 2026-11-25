import { HttpErrorResponse } from '@angular/common/http';
import { Component, ElementRef, inject, signal, ViewChild, WritableSignal } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';

import { SpaceModel } from '../../../core/models/classes/space.model';
import { WarehouseModel } from '../../../core/models/classes/warehouse.model';
import { IApiResponse } from '../../../core/models/interfaces/api-response.model';
import { SpaceService } from '../../../core/services/space-service';

@Component({
  imports: [ReactiveFormsModule],
  selector: 'app-space-modal',
  styleUrl: './space-modal.css',
  templateUrl: './space-modal.html',
})
export class SpaceModal {
  @ViewChild('spaceDialog') spaceDialog?: ElementRef<HTMLDialogElement>;

  selectedWarehouse: WarehouseModel | null = null;
  selectedSpaceId = 0;
  spaceList: WritableSignal<SpaceModel[]> = signal<SpaceModel[]>([]);
  spaceForm!: FormGroup;

  spaceService = inject(SpaceService);
  formBuilder = inject(FormBuilder);

  constructor() {
    this.initializeForm();
  }

  initializeForm(): void {
    this.spaceForm = this.formBuilder.group({
      spaceName: ['', [Validators.required, Validators.maxLength(80)]],
      areaSqft: [1, [Validators.required, Validators.min(1), Validators.pattern(/^\d+$/)]],
      description: ['', Validators.maxLength(250)],
      isActive: [true],
    });
  }

  getAllSpace(warehouse?: WarehouseModel): void {
    if (warehouse) {
      this.selectedWarehouse = warehouse;
      this.spaceList.set([]);
      this.onClearForm();

      if (!this.spaceDialog?.nativeElement.open) {
        this.spaceDialog?.nativeElement.showModal();
      }
    }

    if (!this.selectedWarehouse) {
      return;
    }

    const warehouseId = this.selectedWarehouse.warehouseId;

    this.spaceService.getByWarehouseId(warehouseId).subscribe({
      next: (res: IApiResponse<SpaceModel[]>) => {
        if (res.isSuccess && this.selectedWarehouse?.warehouseId === warehouseId) {
          this.spaceList.set(res.data ?? []);
        }
      },
      error: (err: HttpErrorResponse) => {
        alert(err.error.errorMessages);
      },
    });
  }

  getSpaceById(): void {
    const id = this.selectedSpaceId;

    this.spaceService.getById(id).subscribe({
      next: (res: IApiResponse<SpaceModel>) => {
        if (res.isSuccess && res.data && this.selectedSpaceId === id) {
          this.spaceForm.patchValue(res.data);
        }
      },
      error: (err: HttpErrorResponse) => {
        alert(err.error.errorMessages);
      },
    });
  }

  OnSaveSpace(): void {

    const formValue: SpaceModel = this.spaceForm.value;
    formValue.warehouseId = this.selectedWarehouse!.warehouseId;

    if (this.selectedSpaceId > 0) {
      this.updateSpace(formValue);
    } else {
      this.createSpace(formValue);
    }
  }

  onEditSpace(id: number): void {
    this.selectedSpaceId = id;
    this.getSpaceById();
  }

  createSpace(formValue: SpaceModel): void {
    this.spaceService.create(formValue).subscribe({
      next: (res: IApiResponse<SpaceModel>) => {
        if (res.isSuccess) {
          alert(res.message);
          this.onClearForm();
          this.getAllSpace();
        }
      },
      error: (err: HttpErrorResponse) => {
        alert(err.error.errorMessages);
      },
    });
  }

  updateSpace(formValue: SpaceModel, id = this.selectedSpaceId): void {
    this.spaceService.update(id, formValue).subscribe({
      next: (res: IApiResponse<SpaceModel>) => {
        if (res.isSuccess) {
          alert(res.message);

          if (this.selectedSpaceId === id) {
            this.onClearForm();
          }

          this.getAllSpace();
        }
      },
      error: (err: HttpErrorResponse) => {
        alert(err.error.errorMessages);
      },
    });
  }

  // switch inactive space -> active
  onActivateSpace(id: number): void {
  const space = this.spaceList().find(item => item.spaceId === id);
  if (!space) return;

  const formValue: SpaceModel = { ...space, isActive: true };
  this.updateSpace(formValue, id);
}

  onDeleteSpace(id: number): void {
    const isDelete = confirm('Are you sure you want to deactivate this space?');

    if (!isDelete) {
      return;
    }

    this.spaceService.delete(id).subscribe({
      next: (res: IApiResponse<null>) => {
        if (res.isSuccess) {
          alert(res.message);

          if (this.selectedSpaceId === id) {
            this.onClearForm();
          }

          this.getAllSpace();
        }
      },
      error: (err: HttpErrorResponse) => {
        alert(err.error.errorMessages);
      },
    });
  }

  onClearForm(): void {
    this.selectedSpaceId = 0;
    this.spaceForm.reset({
      spaceName: '',
      areaSqft: 1,
      description: '',
      isActive: true,
    });
  }
}