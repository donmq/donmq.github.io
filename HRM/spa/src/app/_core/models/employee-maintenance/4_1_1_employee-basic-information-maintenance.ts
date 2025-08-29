import { Pagination } from '@utilities/pagination-utility';

export interface EmployeeBasicInformationMaintenanceDto {
  useR_GUID: string;
  nationality: string;
  identificationNumber: string;
  issuedDate: Date;
  issuedDateStr: string;
  company: string;
  employmentStatus: string;
  division: string;
  factory: string;
  employeeID: string;
  department: string;
  assignedDivision: string;
  assignedFactory: string;
  assignedEmployeeID: string;
  assignedDepartment: string;
  permissionGroup: string;
  crossFactoryStatus: string;
  performanceAssessmentResponsibilityDivision: string;
  identityType: string;
  supervisorType: string;
  localFullName: string;
  preferredEnglishFullName: string;
  chineseName: string;
  passportFullName: string;
  gender: string;
  bloodType: string;
  maritalStatus: string;
  dateOfBirth: Date;
  dateOfBirthStr: string;
  phoneNumber: string;
  mobilePhoneNumber: string;
  education: string;
  religion: string;
  transportationMethod: string;
  vehicleType: string;
  licensePlateNumber: string;
  numberOfDependents: number | null;
  registeredProvinceDirectly: string;
  registeredCity: string;
  registeredAddress: string;
  mailingProvinceDirectly: string;
  mailingCity: string;
  mailingAddress: string;
  workShiftType: string;
  swipeCardOption: boolean;
  swipeCardNumber: string;
  positionGrade: number;
  positionTitle: string;
  workType: string;
  restaurant: string;
  // salaryType: string;
  // salaryPaymentMethod: string;
  workLocation: string;
  unionMembership: boolean | null;
  work8hours: boolean | null;
  onboardDate: Date;
  onboardDateStr: string;
  dateOfGroupEmployment: Date;
  dateOfGroupEmploymentStr: string;
  seniorityStartDate: Date;
  seniorityStartDateStr: string;
  annualLeaveSeniorityStartDate: Date;
  annualLeaveSeniorityStartDateStr: string;
  dateOfResignation: string;
  dateOfResignationStr: string;
  reasonResignation: string;
  blacklist: boolean | null;
  updateTime: string;
  updateBy: string;
}

export interface EmployeeBasicInformationMaintenanceParam {
  nationality: string;
  identificationNumber: string;
  employmentStatus: string;
  division: string;
  factory: string;
  employeeID: string;
  department: string;
  assignedDivision: string;
  assignedFactory: string;
  assignedEmployeeID: string;
  assignedDepartment: string;
  crossFactoryStatus: string;
  performanceDivision: string;
  localFullName: string;
  workShiftType: string;
  onboardDate: string;
  onboardDateStr: string;
  dateOfGroupEmployment: string;
  dateOfGroupEmploymentStr: string;
  seniorityStartDate: string;
  seniorityStartDateStr: string;
  annualLeaveSeniorityStartDate: string;
  annualLeaveSeniorityStartDateStr: string;
  dateOfResignation: string;
  dateOfResignationStr: string;
  language: string;

}

export interface checkDuplicateParam {
  division: string;
  factory: string;
  employeeID: string;
}

export interface CheckBlackList {
  useR_GUID: string;
  nationality: string;
  identification_Number: string;
}

export interface EmployeeBasicInformationMaintenanceSource {
  mode: string;
  useR_GUID: string;
  division: string;
  factory: string;
  employee_ID: string;
  nationality: string;
  identificationNumber: string;
  localFullName: string;
  employmentStatus: string;
}

export interface EmployeeBasicInformationMaintenanceView {
  useR_GUID: string;
  employmentStatus: string;
  nationality: string;
  identificationNumber: string;
  localFullName: string;
  division: string;
  factory: string;
  employeeID: string;
  department: string;
  crossFactoryStatus: string;
  onboardDate: string;
  dateOfResignation: string;
  enableRehire: boolean;
}

export interface DepartmentSupervisorList {
  supervisorType: string;
  departmentSupervisor: string;
}

export interface EmployeeBasicInformationMaintenanceMainMemory {
  param: EmployeeBasicInformationMaintenanceParam;
  pagination: Pagination;
  data: EmployeeBasicInformationMaintenanceView[];
}

export interface UploadResultDto {
  total: number;
  success: number;
  error: number;
  errorReport: string;
}
