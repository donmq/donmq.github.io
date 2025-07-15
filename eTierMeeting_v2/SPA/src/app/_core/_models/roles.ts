export interface Roles {
  id: number;
  roleName: string;
  updateBy: string;
  updateTime: string | null;
  roleSequence: number | null;
}


export interface RolesDTO {
  id: number;
  roleName: string;
  updateBy: string;
  updateTime: string | null;
  roleSequence: number | null;
  checked: boolean;
  visible: boolean | true;
}
