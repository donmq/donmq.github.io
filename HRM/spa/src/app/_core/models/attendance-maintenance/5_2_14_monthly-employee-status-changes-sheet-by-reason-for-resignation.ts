export interface MonthlyEmployeeStatusChangesSheet_ByReasonForResignationParam {
    factory: string;
    yearMonth: string;
    permisionGroups: string[];
    language: string;
}


export interface MonthlyEmployeeStatusChangesSheet_ByReasonForResignationResult {
    totalRecords: number;
    data: MonthlyEmployeeStatusChangesSheet_ByReasonForResignationValue[];
}



export interface MonthlyEmployeeStatusChangesSheet_ByReasonForResignationValue {
    headerTitle: string;
    numberOfEmployeesAt: number;
    newHiresThisMonth: number;
    resignationsThisMonth: number;
    totalNumberOfEmployeesAt: number;
}
