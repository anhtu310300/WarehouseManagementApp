export interface IApiResponse<T = unknown> {
  data: T | null;
  message: string;
  errorMessages: string[];
  isSuccess: boolean;
}
