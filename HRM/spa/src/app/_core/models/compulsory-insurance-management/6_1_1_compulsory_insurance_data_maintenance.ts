import { Pagination } from "@utilities/pagination-utility";

export interface CompulsoryInsuranceDataMaintenanceDto {
    useR_GUID: string;
    factory: string;
    employee_ID: string;
    local_Full_Name: string;
    insurance_Type: string;
    insurance_Type_Name: string;
    insurance_Start: string;
    insurance_End: string;
    insurance_Num: string;
    update_By: string;
    update_Time: string;
}

export interface CompulsoryInsuranceDataMaintenanceParam {
    factory: string;
    employee_ID: string;
    insurance_Type: string;
    insurance_Start: string;
    insurance_End: string;
    language: string;
    searchMethod: string;
}
export interface CompulsoryInsuranceDataMaintenance_Basic {
  param: CompulsoryInsuranceDataMaintenanceParam
  pagination: Pagination
  data: CompulsoryInsuranceDataMaintenanceDto
}

export interface CompulsoryInsuranceDataMaintenance_Personal {
    useR_GUID: string;
    local_Full_Name: string;
}

export interface CompulsoryInsuranceDataMaintenance_Upload {
    file: File;
    language: string;
}