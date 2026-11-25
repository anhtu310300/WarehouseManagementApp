export interface ICreatePaymentRequest {
  bookingId: number;
  paymentAmount: number;
  paymentMode: string;
  paymentNarration: string;
  status: string;
  receivedId: number;
}

export interface IUpdatePaymentRequest extends ICreatePaymentRequest {}

export interface IPaymentResponse extends ICreatePaymentRequest {
  paymentId: number;
  paymentDate: string;
}
