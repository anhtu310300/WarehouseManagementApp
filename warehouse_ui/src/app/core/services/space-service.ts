import { Injectable } from '@angular/core';
import { GLOBAL_CONSTANT } from '../constant/global.constant';
import { SpaceModel } from '../models/classes/space.model';
import { IApiResponse } from '../models/interfaces/api-response.model';
import { GenericCrudService } from './generic-crud-service';
import { Observable } from 'rxjs';
import { ISpaceAvailabilityResponse } from '../models/interfaces/space.model';

@Injectable({
  providedIn: 'root',
})
export class SpaceService extends GenericCrudService<SpaceModel> {
  constructor() {
    super(GLOBAL_CONSTANT.API_METHODS.SPACES);
  }

  getByWarehouseId(warehouseId: number): Observable<IApiResponse<SpaceModel[]>> {
    return this.http.get<IApiResponse<SpaceModel[]>>(this.resourceUrl, {
      params: { warehouseId },
    });
  }

   getAvailability(
    warehouseId: number,
    fromDate: string,
    tillDate: string,
  ): Observable<IApiResponse<ISpaceAvailabilityResponse[]>> {
    return this.http.get<IApiResponse<ISpaceAvailabilityResponse[]>>(
      `${this.resourceUrl}/availability`,
      {
        params: {
          warehouseId,
          fromDate,
          tillDate,
        },
      },
    );
  }
}