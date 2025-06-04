import { Pagination } from "@utilities/pagination-utility";

export interface AllowLeaveSundayDto {
  empID: number;
  partName: string;
  deptCode: string;
  deptName: string;
  empName: string;
  empNumber: string;
  isSun: boolean | null;
}

export interface AllowLeaveSundayParam {
  partId: number | null;
  keyword: string;
}

export interface AllowLeaveSundaySource {
  param: AllowLeaveSundayParam
  pagination: Pagination
}