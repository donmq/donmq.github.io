import { LoggedRoles } from "./logged-roles.model";

export interface Users {
  userID: number;
  userName: string;
  hashPass: string;
  hashImage: string;
  emailAddress: string;
  userRank: number | null;
  iSPermitted: boolean | null;
  empID: number | null;
  visible: boolean | null;
  updated: string | null;
  fullName: string;

  roles: LoggedRoles[];
  username: string; // Token user
}