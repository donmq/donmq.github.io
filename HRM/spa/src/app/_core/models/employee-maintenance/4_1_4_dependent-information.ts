export interface HRMS_Emp_Dependent {
    nationality: string;
    identification_Number: string;
    local_Full_Name: string;
    useR_GUID: string;
    seq: number;
    name: string;
    relationship: string;
    relationship_Name: string;
    occupation: string;
    dependents: boolean;
    update_By: string;
    update_Time: string;
}

export interface HRMS_Emp_DependentParam {
  nationality: string;
  identification_Number: string;
  useR_GUID: string;
  lang: string
}
