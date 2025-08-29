import { Pagination } from "@utilities/pagination-utility";

export interface GradeMaintenanceParam {
    type_Code: string;
    level: string;
    level_Code: string;
    language: string;
}

export interface ParamInMain {
    param: GradeMaintenanceParam;
    pagination : Pagination;
    data: HRMS_Basic_Level[]
}

export interface HRMS_Basic_Level {
    level: number;
    level_Code: string;
    type_Code: string;
    type_Code_Name: string;
    update_By: string;
    update_Time: string | null;
    level_Code_Name: string;
    isActive: boolean;
}
