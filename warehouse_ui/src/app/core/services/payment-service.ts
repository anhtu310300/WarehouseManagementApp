import { Injectable } from '@angular/core';
import { GLOBAL_CONSTANT } from '../constant/global.constant';
import { PaymentModel } from '../models/classes/payment.model';
import { GenericCrudService } from './generic-crud-service';

@Injectable({
  providedIn: 'root',
})
export class PaymentService extends GenericCrudService<PaymentModel> {
  constructor() {
    super(GLOBAL_CONSTANT.API_METHODS.PAYMENTS);
  }
}
