export interface API_Response<T = any> {
  data: T | null;
  message: string;
  errorMessages: string[];
  isSuccess: boolean;
}
