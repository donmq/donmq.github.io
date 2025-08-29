export interface HRMSEmpExternalExperience {
  useR_GUID: string;
  nationality: string;
  identification_Number: string;
  local_Full_Name: string;
  seq: number;
  company_Name: string;
  department: string;
  leadership_Role: boolean;
  position_Title: string;
  tenure_Start: string;
  tenure_End: string;
  tenure_Yealy: number;
  update_By: string;
  update_Time: string;
}

export interface HRMSEmpExternalExperienceModel {
  useR_GUID: string;
  nationality: string;
  identification_Number: string;
  local_Full_Name: string;
  seq: number;
  company_Name: string;
  department: string;
  leadership_Role: boolean;
  position_Title: string;
  tenure_Start: string;
  tenure_End: string;
}

export interface HRMS_Emp_External_ExperienceParam {
  useR_GUID: string;
}

