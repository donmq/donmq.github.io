
using API.Models;

namespace API.DTOs.EmployeeMaintenance
{
    public class EmployeeBasicInformationMaintenanceDto
    {
        public string USER_GUID { get; set; }
        public string Nationality { get; set; }
        public string IdentificationNumber { get; set; }
        public DateTime IssuedDate { get; set; }
        public string IssuedDateStr { get; set; }
        public string Company { get; set; }
        public string EmploymentStatus { get; set; }
        public string Division { get; set; }
        public string Factory { get; set; }
        public string EmployeeID { get; set; }
        public string Department { get; set; }
        public string AssignedDivision { get; set; }
        public string AssignedFactory { get; set; }
        public string AssignedEmployeeID { get; set; }
        public string AssignedDepartment { get; set; }
        public string PermissionGroup { get; set; }
        public string CrossFactoryStatus { get; set; }
        public string PerformanceAssessmentResponsibilityDivision { get; set; }
        public string IdentityType { get; set; }
        // public string SupervisorType { get; set; }
        public string LocalFullName { get; set; }
        public string PreferredEnglishFullName { get; set; }
        public string ChineseName { get; set; }
        public string PassportFullName { get; set; }
        public string Gender { get; set; }
        public string BloodType { get; set; }
        public string MaritalStatus { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string DateOfBirthStr { get; set; }
        public string PhoneNumber { get; set; }
        public string MobilePhoneNumber { get; set; }
        public string Education { get; set; }
        public string Religion { get; set; }
        public string TransportationMethod { get; set; }
        public string VehicleType { get; set; }
        public string LicensePlateNumber { get; set; }
        public int? NumberOfDependents { get; set; }
        public string RegisteredProvinceDirectly { get; set; }
        public string RegisteredCity { get; set; }
        public string RegisteredAddress { get; set; }
        public string MailingProvinceDirectly { get; set; }
        public string MailingCity { get; set; }
        public string MailingAddress { get; set; }
        public string WorkShiftType { get; set; }
        public bool SwipeCardOption { get; set; }
        public string SwipeCardNumber { get; set; }
        public decimal PositionGrade { get; set; }
        public string PositionTitle { get; set; }
        public string WorkType { get; set; }
        public string Restaurant { get; set; }
        // public string SalaryType { get; set; }
        // public string SalaryPaymentMethod { get; set; }
        public string WorkLocation { get; set; }
        public bool? UnionMembership { get; set; }
        public bool? Work8hours { get; set; }
        public DateTime OnboardDate { get; set; }
        public string OnboardDateStr { get; set; }
        public DateTime DateOfGroupEmployment { get; set; }
        public string DateOfGroupEmploymentStr { get; set; }
        public DateTime SeniorityStartDate { get; set; }
        public string SeniorityStartDateStr { get; set; }
        public DateTime AnnualLeaveSeniorityStartDate { get; set; }
        public string AnnualLeaveSeniorityStartDateStr { get; set; }
        public DateTime? DateOfResignation { get; set; }
        public string DateOfResignationStr { get; set; }
        public string ReasonResignation { get; set; }
        public bool? Blacklist { get; set; }
        public DateTime UpdateTime { get; set; }
        public string UpdateBy { get; set; }
    }

    public class EmployeeBasicInformationMaintenanceView
    {
        public string USER_GUID { get; set; }
        public string EmploymentStatus { get; set; }
        public string Nationality { get; set; }
        public string IdentificationNumber { get; set; }
        public string LocalFullName { get; set; }
        public string Division { get; set; }
        public string Factory { get; set; }
        public string EmployeeID { get; set; }
        public string Department { get; set; }
        public string CrossFactoryStatus { get; set; }
        public string OnboardDate { get; set; }
        public string DateOfResignation { get; set; }
        public bool EnableRehire { get; set; }
    }

    public class EmployeeBasicInformationReport : EmployeeBasicInformationMaintenanceDto
    {
        public string DepartmentName { get; set; }
        public string AssignedDepartmentName { get; set; }
        public string SalaryType { get; set; }
        public string PositionGradeStr { get; set; }
        public string SwipeCardOptionStr { get; set; }
        public string UnionMembershipStr { get; set; }
        public string UpdateTimeStr { get; set; }
    }

    public class EmployeeBasicInformationMaintenanceParam
    {
        private string _identificationNumber;
        private string _employeeID;
        private string _assignedEmployeeID;
        private string _localFullName;
        public string Nationality { get; set; }
        public string IdentificationNumber { get => _identificationNumber; set => _identificationNumber = value?.Trim(); }
        public string EmploymentStatus { get; set; }
        public string Division { get; set; }
        public string Factory { get; set; }
        public string EmployeeID { get => _employeeID; set => _employeeID = value?.Trim(); }
        public string Department { get; set; }
        public string AssignedDivision { get; set; }
        public string AssignedFactory { get; set; }
        public string AssignedEmployeeID { get => _assignedEmployeeID; set => _assignedEmployeeID = value?.Trim(); }
        public string AssignedDepartment { get; set; }
        public string CrossFactoryStatus { get; set; }
        public string PerformanceDivision { get; set; }
        public string LocalFullName { get => _localFullName; set => _localFullName = value?.Trim(); }
        public string WorkShiftType { get; set; }
        public string OnboardDate { get; set; }
        public string OnboardDateStr { get; set; }
        public string DateOfGroupEmployment { get; set; }
        public string DateOfGroupEmploymentStr { get; set; }
        public string SeniorityStartDate { get; set; }
        public string SeniorityStartDateStr { get; set; }
        public string AnnualLeaveSeniorityStartDate { get; set; }
        public string AnnualLeaveSeniorityStartDateStr { get; set; }
        public string DateOfResignation { get; set; }
        public string DateOfResignationStr { get; set; }
        public string Language { get; set; }
    }


    /// <summary>
    /// Employee Basic Information Report Param
    /// </summary>
    public class EmployeeBasicInformationReportParam
    {
        private string _identificationNumber;
        private string _employeeID;
        private string _assignedEmployeeID;
        public string Nationality { get; set; }
        public string IdentificationNumber { get => _identificationNumber; set => _identificationNumber = value?.Trim(); }
        public string EmploymentStatus { get; set; }
        public string Division { get; set; }
        public string Factory { get; set; }
        public string EmployeeID { get => _employeeID; set => _employeeID = value?.Trim(); }
        public string Department { get; set; }
        public string AssignedDivision { get; set; }
        public string AssignedFactory { get; set; }
        public string AssignedEmployeeID { get => _assignedEmployeeID; set => _assignedEmployeeID = value?.Trim(); }
        public string AssignedDepartment { get; set; }
        public string PermissionGroup { get; set; }
        public string PositionGrade_Start { get; set; }
        public string PositionGrade_End { get; set; }
        public string OnboardDateStartStr { get; set; }
        public string OnboardDateEndStr { get; set; }
        public string DateOfGroupEmploymentStart { get; set; }
        public string DateOfGroupEmploymentEnd { get; set; }
        public string DateOfResignationStart { get; set; }
        public string DateOfResignationEnd { get; set; }
        public string Language { get; set; }
        public string UserName { get; set; }
    }





    public class CheckDuplicateParam
    {
        private string _employeeID;
        public string Division { get; set; }
        public string Factory { get; set; }
        public string EmployeeID { get => _employeeID; set => _employeeID = value?.Trim(); }
    }

    public class CheckBlackList
    {
        public string USER_GUID { get; set; }
        public string Nationality { get; set; }
        public string Identification_Number { get; set; }
    }

    public class DepartmentSupervisorList
    {
        public string SupervisorType { get; set; }
        public string DepartmentSupervisor { get; set; }
    }

    public class SupervisorType
    {
        public string Code { get; set; }
        public string Code_Name { get; set; }
        public string Lang { get; set; }

        public SupervisorType(string code, string code_Name, string lang)
        {
            Code = code;
            Code_Name = code_Name;
            Lang = lang;
        }
    }

    public class EmployeeBasicInformationMaintenanceReport
    {
        public string Nationality { get; set; }
        public string IdentificationNumber { get; set; }
        public string IssuedDate { get; set; }
        public string Division { get; set; }
        public string Factory { get; set; }
        public string EmployeeID { get; set; }
        public string Department { get; set; }
        public string AssignedDivision { get; set; }
        public string AssignedFactory { get; set; }
        public string AssignedEmployeeID { get; set; }
        public string AssignedDepartment { get; set; }
        public string PermissionGroup { get; set; }
        public string CrossFactoryStatus { get; set; }
        public string PerformanceAssessmentResponsibilityDivision { get; set; }
        public string IdentityType { get; set; }
        public string LocalFullName { get; set; }
        public string PreferredEnglishFullName { get; set; }
        public string ChineseName { get; set; }
        public string Gender { get; set; }
        public string BloodType { get; set; }
        public string MaritalStatus { get; set; }
        public string DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string MobilePhoneNumber { get; set; }
        public string Education { get; set; }
        public string Religion { get; set; }
        public string TransportationMethod { get; set; }
        public string VehicleType { get; set; }
        public string LicensePlateNumber { get; set; }
        public string RegisteredProvinceDirectly { get; set; }
        public string RegisteredCity { get; set; }
        public string RegisteredAddress { get; set; }
        public string MailingProvinceDirectly { get; set; }
        public string MailingCity { get; set; }
        public string MailingAddress { get; set; }
        public string WorkShiftType { get; set; }
        public string SwipeCardOption { get; set; }
        public string SwipeCardNumber { get; set; }
        public string PositionGrade { get; set; }
        public string PositionTitle { get; set; }
        public string WorkType { get; set; }
        public string Restaurant { get; set; }
        public string WorkLocation { get; set; }
        public string UnionMembership { get; set; }
        public string OnboardDate { get; set; }
        public string DateOfGroupEmployment { get; set; }
        public string SeniorityStartDate { get; set; }
        public string AnnualLeaveSeniorityStartDate { get; set; }
        public string Error_Message { get; set; }
    }

    public class EmployeeBasicInformationMaintenance_UploadResult
    {
        public int Total { get; set; }
        public int Success { get; set; }
        public int Error { get; set; }
        public byte[] ErrorReport { get; set; }
    }
    public class HRMS_Emp_Personal_Next : HRMS_Emp_Personal
    {
        public string Employee_Status { get; set; }
    }
}