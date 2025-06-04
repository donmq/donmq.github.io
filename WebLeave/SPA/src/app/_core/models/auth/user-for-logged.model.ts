import { Users } from "./users.model";

export interface UserForLogged {
  token: string;
  user: Users;
}

export interface LoginResponse {
  token: string;
  user: UserForLogged;
  alreadyLoggedIn: boolean;
  confirmRelogin: boolean;
}