export interface EmergencyContactsReportExport {
    division: string;
    factory: string;
    employeeID: string;
    localFullName: string;
    seq: string;
    emergencyContact: string;
    relationship: string;
    emergencyContactPhone: string;
    temporaryAddress: string;
    emergencyContactAddress: string;
}
export interface EmergencyContactsReportParam {
    employmentStatus: string;
    division: string;
    factory: string;
    employeeID: string;
    department: string;
    assignedDivision: string;
    assignedFactory: string;
    assignedEmployeeID: string;
    assignedDepartment: string;
    language: string;
}

export interface EmergencyContactsReportSource {
  param: EmergencyContactsReportParam;
  totalRows: number;
}
