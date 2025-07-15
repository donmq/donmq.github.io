export interface UserRoles {
  roles: number;
  empNumber: string;
  updateBy: string;
  updateTime: string | null;
  createBy: string;
  createTime: string;
  active: boolean | null;
}
