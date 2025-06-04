export interface Employee {
    empID: number;
    empName: string;
    empNumber: string;
    dateIn: string | null;
    positionID: number | null;
    descript: string;
    gBID: number | null;
    visible: boolean | null;
    partID: number | null;
    hisrotyID: number;
    yearIn: number | null;
    totalDay: number | null;
    arrange: number | null;
    agent: number | null;
    countArran: number | null;
    countRestArran: number | null;
    countAgent: number | null;
    countRestAgent: number | null;
    countTotal: number | null;
    countLeave: number | null;
    userID: number;
    userName: string;
    hashPass: string;
    hashImage: string;
    emailAddress: string;
    userRank: number | null;
    iSPermitted: boolean | null;
    updated: string | null;
    fullName: string;
    deptID: number | null;
    isCreateAccount: boolean | null;
}

export interface ResultDataUploadEmp {
  ignore: string;
  countCreateEmp: number;
  totalEmp: number;
  countUpdateEmp: number;
}
