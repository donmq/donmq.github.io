import { Pagination } from '@utilities/pagination-utility';

export interface EmployeeTransferOperationInboundDto {
  history_GUID: string;
  useR_GUID: string;
  // Before
  nationalityBefore: string;
  identificationNumberBefore: string;
  localFullNameBefore: string;
  divisionBefore: string;
  factoryBefore: string;
  employeeIDBefore: string;
  departmentBefore: string;
  assignedDivisionBefore: string;
  assignedFactoryBefore: string;
  assignedEmployeeIDBefore: string;
  assignedDepartmentBefore: string;
  positionGradeBefore: number;
  positionTitleBefore: string;
  workTypeBefore: string;
  reasonForChangeBefore: string;
  effectiveDateBefore: Date;
  effectiveDateBeforeStr: string;
  effectiveStatusBefore: boolean;
  updateTimeBefore: string;
  updateByBefore: string;
  // After
  nationalityAfter: string;
  identificationNumberAfter: string;
  localFullNameAfter: string;
  divisionAfter: string;
  factoryAfter: string;
  employeeIDAfter: string;
  departmentAfter: string;
  assignedDivisionAfter: string;
  assignedFactoryAfter: string;
  assignedEmployeeIDAfter: string;
  assignedDepartmentAfter: string;
  positionGradeAfter: number;
  positionTitleAfter: string;
  workTypeAfter: string;
  reasonForChangeAfter: string;
  effectiveDateAfter: Date | null;
  effectiveDateAfterStr: string;
  effectiveStatusAfter: boolean;
  updateTimeAfter: string;
  updateByAfter: string;
}

export interface EmployeeTransferOperationInboundParam {
  nationality: string;
  identificationNumber: string;
  division: string;
  factory: string;
  employeeID: string;
  localFullName: string;
  assignedDivision: string;
  assignedFactory: string;
  assignedEmployeeID: string;
  reasonForChange: string;
  effectiveDateStart: string;
  effectiveDateEnd: string;
  effectiveDateStart_Str: string;
  effectiveDateEnd_Str: string;
  effectiveStatus: string;
  language: string;
}

export interface EmployeeTransferOperationInboundMainMemory {
  param: EmployeeTransferOperationInboundParam;
  pagination: Pagination;
  data: EmployeeTransferOperationInboundDto[];
}
