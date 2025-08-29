import { Pagination } from "@utilities/pagination-utility";

export interface FemaleEmpMenstrualMain {
    id?: string;
    useR_GUID: string;
    factory: string;
    att_Month: Date;
    att_Month_Str: string;
    breaks_Date: Date;
    breaks_Date_Str: string;
    employee_ID: string;
    local_Full_Name: string;
    department_Code: string;
    department_Code_Name: string;
    department_Name: string;
    onboard_Date: Date;
    onboard_Date_Str: string;
    time_Start: string;
    time_End: string;
    breaks_Hours: number;
    update_By: string;
    update_Time: Date;
    update_Time_Str: string;
    isDuplicate?: boolean;
    language: string
}

export interface EmpMenstrualInfo {
    useR_GUID: string;
    local_Full_Name: string;
    department: string;
    department_Name: string;
    onboard_Date: string;
}

export interface FemaleEmpMenstrualParam {
    factory: string;
    employee_ID: string;
    att_Month: string;
    att_Month_Str: string;
    department: string;
    language: string;
    action?: string;
    useR_GUID: string;
}

export interface EmpMenstrualInfoParam {
    factory: string;
    employee_ID: string;
    language: string;
}

export interface FemaleEmpMenstrualMemory {
    params: FemaleEmpMenstrualParam;
    pagination: Pagination;
    datas: FemaleEmpMenstrualMain[];
}
