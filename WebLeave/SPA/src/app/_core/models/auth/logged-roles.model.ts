export interface LoggedRoles {
  roleSym: string;
  route: string;
  imgSrc: string;
  badge: number;
  subRoles: LoggedRoles[];
}