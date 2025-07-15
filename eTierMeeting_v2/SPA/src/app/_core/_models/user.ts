export interface UserForLoginDto {
  username: string;
  password: string;
}

export interface User {
  userID: number;
  userName: string;
  hashPass: string;
  hashImage: string;
  emailAddress: string;
  visible: boolean | null;
  updateDate: string | null;
  updateBy: string;
  empName: string;
  roles: number[];
}
