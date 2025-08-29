import { BaseSource } from "@models/base-source";

export interface HRMS_Basic_Code {
    type_Seq: string;
    code: string;
    code_Name: string;
    char1: string;
    char2: string;
    date1: string | null;
    date2: string | null;
    date3: string | null;
    int1: number | null;
    int2: number | null;
    int3: number | null;
    decimal1: number | null;
    decimal2: number | null;
    decimal3: number | null;
    remark: string;
    remark_Code: string;
    isActive: boolean;
    update_By: string;
    update_Time: string | null;
    update_Time_String: string | null;
    type_Name: string;
    state: string;
    seq: string;
}

export interface CodeMaintenanceParam {
    type_Seq: string;
    type_Name: string;
    code: string;
    code_Name: string;
}

export interface HRMS_Basic_Code_Source extends BaseSource<HRMS_Basic_Code> {
    // Dữ liệu Page Main
    param: CodeMaintenanceParam,
}
