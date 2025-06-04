export interface PermissionRightsDTO {
    stt: number;
    empNumber: string;
    empName: string;
    positionName: string;
    part: string;
    approvalUsers: string;
}

export interface PermissionParam {
    empNumber: string;
    partID: number;
    // title excel
    label_Stt: string;
    label_EmpNumber: string;
    label_EmpName: string;
    label_PositionName: string;
    label_Part: string;
    label_ApprovalUsers: string;

}
