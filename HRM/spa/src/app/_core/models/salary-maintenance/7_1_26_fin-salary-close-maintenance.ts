import { Pagination } from "@utilities/pagination-utility";

export interface FinSalaryCloseMaintenance_Param {
    factory: string;
    permission_Group: string[];
    year_Month: string;
    employee_ID: string;
    department: string;
    kind: string;
    language: string;
}
export interface FinSalaryCloseMaintenance_MainData {
    factory: string;
    year_Month: Date;
    year_Month_Str: string;
    department: string;
    department_Name: string;
    employee_ID: string;
    local_Full_Name: string;
    permission_Group: string;
    close_Status: string;
    close_End_Str: Date;
    close_End: string;
    onboard_Date: Date;
    onboard_Date_Str: string;
    resign_Date: Date;
    resign_Date_Str: string;
    update_By: string;
    update_Time: string;
}

export interface FinSalaryCloseMaintenance_UpdateParam {
    factory: string;
    year_Month: string;
    employee_ID: string;
    close_Status: string;
    update_By: string;
}
export interface BatchUpdateData_Param {
    factory: string;
    kind: string;
    year_Month: string;
    close_Status: string;
    permission_Group: string[]
}

export interface FinSalaryCloseMaintenance_Memory {
    salaryCloseSearch_Param: FinSalaryCloseMaintenance_Param;
    batchUpdateData_Param: BatchUpdateData_Param;
    salaryCloseSearch_Data: FinSalaryCloseMaintenance_MainData[];
    pagination: Pagination;
    selectedData: FinSalaryCloseMaintenance_MainData;
    selectedTab: string;
}  