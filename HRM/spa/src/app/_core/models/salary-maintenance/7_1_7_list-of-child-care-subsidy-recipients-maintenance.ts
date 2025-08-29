import { Pagination } from "@utilities/pagination-utility";

export interface HRMS_Sal_Childcare_SubsidyDto {
    useR_GUID: string;
    factory: string;
    department_Code: string;
    department_Name: string;
    department_Code_Name: string;
    employee_ID: string;
    employee_ID_Old: string;
    local_Full_Name: string;
    birthday_Child: Date | string;
    month_Start: Date | string;
    month_End: Date | string;
    num_Children: number;
    update_By: string;
    update_Time: string;
}

export interface ListofChildcareSubsidyRecipientsMaintenanceParam {
  factory:string;
  employee_ID: string;
  language: string;
}

export interface DeleteParam {
    factory: string;
    employee_ID: string;
    birthday_Child: Date | string;
}

export interface ExistedDataParam extends DeleteParam{
}

export interface ListofChildcareSubsidyRecipientsMaintenanceSource {
  param: ListofChildcareSubsidyRecipientsMaintenanceParam
  pagination: Pagination
  selectedData: HRMS_Sal_Childcare_SubsidyDto
  data: HRMS_Sal_Childcare_SubsidyDto[]
}
