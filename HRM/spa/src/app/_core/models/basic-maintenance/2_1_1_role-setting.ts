import { TreeviewItem } from "@ash-mezdo/ngx-treeview";
import { Pagination } from "@utilities/pagination-utility";

export interface RoleSetting {
  role: string;
  description: string;
  factory: string;
  permission_Group: string;
  level_Start: string;
  level_End: string;
  direct: string;
  direct_Str: string;
}
export interface RoleSettingParam {
  role: string;
  description: string;
  factory: string;
  permission_Group: string;
  direct: string;
  lang: string;
}
export interface RoleSettingDetail extends RoleSetting {
  update_By: string;
  update_Time: Date | null;
  update_Time_Str: string;
}
export interface RoleSettingDto {
  role_Setting: RoleSetting;
  role_List: TreeviewItem[];
}
export interface RoleSetting_MainMemory {
  param: RoleSettingParam
  pagination: Pagination
  data: RoleSettingDetail[]
}
