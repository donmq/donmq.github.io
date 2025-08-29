import { Pagination } from '@utilities/pagination-utility';

export interface ExitEmployeeMasterFileHistoricalDataDto {
  useR_GUID: string;
  nationality: string;
  identificationNumber: string;
  issuedDate: Date; //new
  issuedDateStr: string;
  company: string; //new
  deletionCode: string;
  division: string;
  factory: string;
  employeeID: string;
  department: string;
  assignedDivision: string;
  assignedFactory: string;
  assignedEmployeeID: string;
  assignedDepartment: string;
  permissionGroup: string; //new
  employmentStatus: string;
  performanceAssessmentResponsibilityDivision: string;
  identityType: string; //new
  supervisorType: string; //new
  departmentSupervisorList: DepartmentSupervisorList[]; //new
  localFullName: string;
  preferredEnglishFullName: string; //new
  chineseName: string; //new
  passportFullName: string; //new
  gender: string; //new
  bloodType: string; //new
  maritalStatus: string; //new
  dateOfBirth: Date; //new
  dateOfBirthStr: string;
  phoneNumber: string; //new
  mobilePhoneNumber: string; //new
  education: string; //new
  religion: string; //new
  transportationMethod: string; //new
  vehicleType: string; //new
  licensePlateNumber: string; //new
  numberOfDependents: number | null; //new
  registeredProvinceDirectly: string; //new
  registeredCity: string; //new
  registeredAddress: string; //new
  mailingProvinceDirectly: string; //new
  mailingCity: string; //new
  mailingAddress: string; //new
  workShiftType: string; //new
  swipeCardOption: boolean; //new
  swipeCardNumber: string; //new
  positionGrade: number; //new
  positionTitle: string; //new
  workType: string; //new
  restaurant: string; //new
  // salaryType: string; //new
  // salaryPaymentMethod: string; //new
  workLocation: string; //new
  unionMembership: boolean | null; //new
  onboardDate: Date;
  onboardDateStr: string;
  dateOfGroupEmployment: Date;
  dateOfGroupEmploymentStr: string;
  seniorityStartDate: Date;
  seniorityStartDateStr: string;
  annualLeaveSeniorityStartDate: Date; //new
  annualLeaveSeniorityStartDateStr: string; //new
  dateOfResignation: string;
  dateOfResignationStr: string;
  reasonResignation: string; //new
  blacklist: boolean | null; //new
  updateTime: string; //new
  updateBy: string; //new
}

export interface ExitEmployeeMasterFileHistoricalDataParam {
  nationality: string;
  identificationNumber: string;
  division: string;
  factory: string;
  employeeID: string;
  localFullName: string;
  onboardDateStart: string;
  onboardDateEnd: string;
  onboardDateStart_Str: string;
  onboardDateEnd_Str: string;
  groupDateStart: string;
  groupDateEnd: string;
  groupDateStart_Str: string;
  groupDateEnd_Str: string;
  resignDateStart: string;
  resignDateEnd: string;
  resignDateStart_Str: string;
  resignDateEnd_Str: string;
  language: string;
}

export interface checkDuplicateParam {
  division: string;
  factory: string;
  employeeID: string;
}

export interface ExitEmployeeMasterFileHistoricalDataView {
  useR_GUID: string;
  nationality: string;
  identificationNumber: string;
  localFullName: string;
  division: string;
  factory: string;
  employeeID: string;
  department: string;
  onboardDate: string;
  groupDate: string;
  resignDate: string;
  employmentStatus: string;
  deletionCode : string;
}

export interface DepartmentSupervisorList {
  supervisorType: string;
  departmentSupervisor: string;
}

export interface ExitEmployeeMasterFileHistoricalDataMainMemory {
  param: ExitEmployeeMasterFileHistoricalDataParam;
  pagination: Pagination;
  data: ExitEmployeeMasterFileHistoricalDataView[];
}

export interface ExitEmployeeMasterFileHistoricalDataSource {
  useR_GUID: string;
  resignDate: string;
}
