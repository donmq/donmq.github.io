export interface Employee {
    empID: number;
    empName: string;
    empNumber: string;
    dateIn: Date | null;
    positionID: number | null;
    descript: string;
    gbid: number | null;
    visible: boolean  ;
    partID: number | null;
    deptID: number;
}
export interface ListEmployee {
    deptCode: string;
    partID: number | null;
    partName: string;
    positionSym: string;
    empID: number;
    empName: string;
    empNumber: string;
    visible: boolean ;
}

export interface EmployeeRedirect {
    key: string;
    url: string;
}
