import { ILoginRequest } from '../interfaces/auth.model';

export class UserLoginModel implements ILoginRequest {
  userName = '';
  password = '';
}
