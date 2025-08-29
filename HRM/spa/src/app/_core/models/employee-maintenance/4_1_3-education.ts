export interface HRMS_Emp_Educational {
    useR_GUID: string;
    nationality: string;
    identification_Number: string;
    local_Full_Name: string;
    degree: string;
    degreeName: string;
    academic_System: string;
    academic_SystemName: string;
    major: string;
    majorName: string;
    school: string;
    department: string;
    period_Start: string;
    period_End: string;
    graduation: boolean;
    update_By: string;
    update_Time: string;
}

export interface HRMS_Emp_Educational_FileUpload {
    uSER_GUID: string;
    serNum: string;
    fileID: number;
    fileName: string;
    fileSize: number;
    update_By: string;
    update_Time: string;
}

export interface HRMS_Emp_EducationalParam {
    useR_GUID: string;
    language: string;
}

export interface EducationUpload {
    useR_GUID: string;
    mode: string;
    files: EducationFile[];
}

export interface EducationFile {
    uSER_GUID: string;
    fileID: number; // File ID
    serNum: string;

    fileName: string;
    fileSize: number;
    file: File; // File Upload

    isDownload: boolean;

}
