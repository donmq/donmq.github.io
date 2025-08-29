
export interface NewResignedEmployeeDataPrintingParam {
  factory: string;
  kind: string;
  date_From: string;
  date_To: string;
  department: string;
  lang: string;
}

export interface NewResignedEmployeeDataPrintingDto {
  department: string;
  department_Name: string;
  employee_ID: string;
  local_Full_Name: string;
  position_Title: string;
  onboard_Date: string;
  resign_Date: string;
  resign_Reason: string;
  education: string;
  registered_Address: string;
  birthday: string;
  transportation_Method: string;
  work_Type: string;
  mobile_Phone_Number: string;
  gender: string;
  contract_Date: string;
  insurance_Date: string;
  seniority: number;
  lang: string;
}

export interface NewResignedEmployeeDataPrintingSource {
  dateFrom: Date;
  dateTo: Date;
  selectedKey: string;
  param: NewResignedEmployeeDataPrintingParam,
  source?: NewResignedEmployeeDataPrintingDto,
  total: number;
}
