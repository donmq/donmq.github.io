import { Pagination } from "@utilities/pagination-utility";

export interface SalaryItemToAccountingCodeMappingMaintenanceDto {
    factory: string;
    salary_Item: string;
    salary_Item_Name: string;
    main_Acc: string;
    sub_Acc: string;
    dC_Code: string;
    update_By: string;
    update_Time: string;
    update_Time_Str: string;
}

export interface SalaryItemToAccountingCodeMappingMaintenanceParam {
    factory: string;
    language: string;
}

export interface SalaryItemToAccountingCodeMappingMaintenance_Main_Memory {
    param: SalaryItemToAccountingCodeMappingMaintenanceParam;
    data: SalaryItemToAccountingCodeMappingMaintenanceDto[];
    selectedData: SalaryItemToAccountingCodeMappingMaintenanceDto;
    pagination : Pagination;
}

