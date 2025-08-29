import { Pagination } from "@utilities/pagination-utility";

export interface RehireEvaluationForFormerEmployeesDto {
  personal: RehireEvaluationForFormerEmployeesPersonal
  evaluation: RehireEvaluationForFormerEmployeesEvaluation
}
export interface RehireEvaluationForFormerEmployeesPersonal {
  useR_GUID: string;
  division: string;
  factory: string;
  department: string;
  employeeID: string;
  local_Full_Name: string;
  onboard_Date: string;
  onboard_Date_Date: string;
  date_of_Resignation: string;
  date_of_Resignation_Date: string;
  resign_Type: string;
  resign_Reason: string;
  blacklist: boolean | null;
  seq: number;
}
export interface RehireEvaluationForFormerEmployeesEvaluation {
  nationality: string;
  identification_Number: string;
  division: string;
  factory: string;
  department: string;
  employeeID: string;
  results: boolean;
  seq: number;
  useR_GUID: string;
  explanation: string;
  update_By: string;
  update_Time: string;
}

export interface RehireEvaluationForFormerEmployeesParam {
  nationality: string;
  identification_Number: string;
}

export interface RehireEvaluationForFormerEmployees {
  param: RehireEvaluationForFormerEmployeesParam
  pagination: Pagination
  data: RehireEvaluationForFormerEmployeesDto[]
}

export interface RehireEvaluationForFormerEmployeesSource {
  formType : string,
  data: RehireEvaluationForFormerEmployeesDto
}
