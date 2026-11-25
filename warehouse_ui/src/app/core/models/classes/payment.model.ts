import { IPaymentResponse } from '../interfaces/payment.model';

export class PaymentModel implements IPaymentResponse {
  paymentId = 0;
  bookingId = 0;
  paymentDate = '';
  paymentAmount = 0;
  paymentMode = '';
  paymentNarration = '';
  status = '';
  receivedId = 0;
}
