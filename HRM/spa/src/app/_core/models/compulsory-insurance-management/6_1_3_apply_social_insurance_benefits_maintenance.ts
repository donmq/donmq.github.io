import { Pagination } from "@utilities/pagination-utility";

export interface ApplySocialInsuranceBenefitsMaintenanceDto {
    useR_GUID: string;
    factory: string;
    employee_ID: string;
    local_Full_Name: string;
    declaration_Month: Date;
    declaration_Month_Str: string;
    declaration_Seq: number;
    benefits_Kind: string;
    benefits_Name: string;
    special_Work_Type: string;
    work_Type: string;
    birthday_Child: Date;
    birthday_Child_Str: string;
    benefits_Start: Date;
    benefits_Start_Str: string;
    benefits_End: Date;
    benefits_End_Str: string;
    benefits_Num: string;
    total_Days: number;
    amt: number;
    compulsory_Insurance_Number: string;
    annual_Accumulated_Days: number;
    update_By: string;
    update_Time: Date;
    update_Time_Str:string;
    is_Edit: boolean
}

export interface ApplySocialInsuranceBenefitsMaintenanceParam {
    factory: string;
    start_Year_Month: string;
    start_Year_Month_Str: string;
    end_Year_Month: string;
    end_Year_Month_Str: string;
    declaration_Seq: number;
    benefits_Kind: string;
    employee_ID: string;
    language: string;
}
export interface ApplySocialInsuranceBenefitsMaintenance_Basic {
  param: ApplySocialInsuranceBenefitsMaintenanceParam;
  pagination: Pagination;
  selectedData: ApplySocialInsuranceBenefitsMaintenanceDto;
  data: ApplySocialInsuranceBenefitsMaintenanceDto[];
}
