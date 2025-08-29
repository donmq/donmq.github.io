export interface EmployeeEmergencyContactsDto {
    useR_GUID: string;
    division: string;
    factory: string;
    employee_ID: string;
    localFullName: string;
    nationality: string;
    identification_Number: string;
    seq: number;
    emergency_Contact: string;
    relationship: string;
    emergency_Contact_Phone: string;
    temporary_Address: string;
    emergency_Contact_Address: string;
    update_By: string;
    update_Time: Date | string;
}

export interface EmployeeEmergencyContactsParam {
    useR_GUID: string;
    division: string;
    factory: string;
    employee_ID: string;
    language: string;
}

export interface DataMain {
    result: EmployeeEmergencyContactsDto[];
    totalCount: number;
}
