import { Pagination } from "@utilities/pagination-utility";

export interface Org_Direct_Department_Base {
  division: string;
  factory: string;
  department_Code: string;
  line_Code: string;
}
export interface Org_Direct_DepartmentResult extends Org_Direct_Department_Base {
  line_Name: string;
  direct_Department_Attribute: string;
  direct_Department_Attribute_Name: string;
  update_By: string;
  update_Time: string;
}
export interface GetName {
  name: string;
}
export interface Org_Direct_DepartmentParam extends Org_Direct_Department_Base {
  lang?: string;
}
export interface Org_Direct_DepartmentSource {
  pagination: Pagination;
  data: Org_Direct_DepartmentResult[];
  param: Org_Direct_DepartmentParam;
  selectedData: Org_Direct_DepartmentResult;
}
