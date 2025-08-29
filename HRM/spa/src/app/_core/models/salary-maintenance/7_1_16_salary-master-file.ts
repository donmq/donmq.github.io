import { Pagination, PaginationResult } from "@utilities/pagination-utility";

export interface SalaryMasterFile_Main {
    useR_GUID: string;
    division: string;
    factory: string;
    employee_ID: string;
    local_Full_Name: string;
    employment_Status: string;
    department: string;
    department_Str: string;
    position_Grade: number;
    position_Title: string;
    position_Title_Str: string;
    permission_Group: string;
    permission_Group_Str: string;
    actingPosition_Start: Date | string | null;
    actingPosition_End: Date | string | null;
    technical_Type: string;
    technical_Type_Str: string;
    expertise_Category: string;
    expertise_Category_Str: string;
    salary_Type: string;
    salary_Type_Str: string;
    salary_Grade: number;
    salary_Level: number;
    currency: string;
    update_By: string;
    update_Time: Date | string;
    effective_Date: Date | string | null;
    onboard_Date: Date | string | null;
}
export interface SalaryMasterFile_Param {
    factory: string;
    department: string;
    employee_ID: string;
    employment_Status: string;
    position_Title: string;
    permission_Group: string[];
    salary_Type: string;
    salary_Grade: string;
    salary_Level: string;
    language: string;
}
export interface SalaryMasterFile_Detail {
    salaryItemsPagination: PaginationResult<SalaryItem>;
    total_Salary: number;
}

export interface SalaryItem {
    salary_Item: string;
    amount: number;
}

export interface SalaryMasterFile_Main_Memory {
    paramSearch: SalaryMasterFile_Param;
    data: SalaryMasterFile_Main[];
    selectedData: SalaryMasterFile_Main;
    pagination : Pagination;
}
