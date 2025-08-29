import { BaseSource } from '@models/base-source';
import { PaginationResult } from '../../utilities/pagination-utility';

export interface HRMS_Org_Work_Type_Headcount {
    division: string;
    factory: string;
    department_Code: string;
    department_Name: string;
    effective_Date: string;
    work_Type_Code: string;
    work_Type_Name: string;
    approved_Headcount: number;
    actual_Headcount: number;
    update_By: string;
    update_Time: string;

    isEdit: boolean;
    isDelete: boolean;
}

export interface DepartmentNameObject {
    department_Code: string;
    department_Name: string;
}

export interface HRMS_Org_Work_Type_HeadcountDataMain {
    totalApprovedHeadcount: number;
    totalActual: number;
    dataPagination: PaginationResult<HRMS_Org_Work_Type_Headcount>;
}

export interface HRMS_Org_Work_Type_HeadcountUpdate {
    dataUpdate: HRMS_Org_Work_Type_Headcount[];
    dataNewAdd: HRMS_Org_Work_Type_Headcount[];
}

export interface HRMS_Org_Work_Type_HeadcountSource extends BaseSource<HRMS_Org_Work_Type_Headcount> {
    // Dữ liệu Main
    param: HRMS_Org_Work_Type_HeadcountParam;
    effective_Date_value: Date;
    totalHeadCount: number;
    totalActualNumber: number;
}

export interface HRMS_Org_Work_Type_HeadcountParam {
    division: string;
    factory: string;
    department_Code: string;
    effective_Date: string;
    language: string | null;
}
