export interface SalaryApprovalForm_Param {
    factory: string;
    kind: string;
    department: string;
    permission_Group: string[];
    employee_ID: string;
    position_Title?: string;
    language: string;
}

export interface SalaryApprovalForm_Source {
    param : SalaryApprovalForm_Param;
    totalRows: number;
}