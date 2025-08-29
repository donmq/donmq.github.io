import { Pagination } from "@utilities/pagination-utility";

export interface HRMS_Basic_Factory_Comparison {
  kind: string;
  factory: string;
  division: string;
  modified_By: string;
  modification_Date: string;
}

export interface HRMS_Basic_Factory_ComparisonSource {
  pagination: Pagination,
  kind: string,
  factoryComparison: HRMS_Basic_Factory_Comparison
  data: HRMS_Basic_Factory_Comparison[]
}

export interface HRMS_Basic_Factory_ComparisonAdd {
  kind: string;
  factory: string;
  division: string;
  modified_By: string;
  modification_Date: string;
  isValid: boolean;
}
