import { Pagination } from "@utilities/pagination-utility";

export interface IncomeTaxFreeSetting_MainData {
    factory: string;
    type: string;
    type_Name: string;
    salary_Type: string;
    salary_Type_Name: string;
    effective_Month: string;
    effective_Month_Str: string;
    amount: string;
    update_By: string;
    update_Time: Date;
    is_Disabled: boolean;
}

export interface IncomeTaxFreeSetting_MainParam {
    factory: string;
    type: string;
    salary_Type: string;
    start_Effective_Month: string;
    end_Effective_Month: string;
    language: string;
}

export interface IncomeTaxFreeSetting_Form {
    data: IncomeTaxFreeSetting_SubData [];
    param: IncomeTaxFreeSetting_SubParam;
}

export interface IncomeTaxFreeSetting_SubParam {
    factory: string;
    salary_Type: string;
    effective_Month: string;
    effective_Month_Str: string;
}

export interface IncomeTaxFreeSetting_SubData {
    type: string;
    amount: string;
    update_By: string;
    update_Time: Date;
    update_Time_Str: string;
    is_Duplicate: boolean;
    is_Disabled_Edit: boolean;
}

export interface IncomeTaxFreeSettingMemory {
    param: IncomeTaxFreeSetting_MainParam;
    pagination: Pagination;
    data: IncomeTaxFreeSetting_MainData[];
    selectedData : IncomeTaxFreeSetting_MainData
}
