
namespace API.DTOs.EmployeeMaintenance
{
    public class ExitEmployeeMasterFileHistoricalDataDto
    {
        public string USER_GUID { get; set; }
        public string Nationality { get; set; }
        public string IdentificationNumber { get; set; }
        public DateTime IssuedDate { get; set; } //new 
        public string IssuedDateStr { get; set; }
        public string Company { get; set; } //new
        public string DeletionCode { get; set; }
        public string Division { get; set; }
        public string Factory { get; set; }
        public string EmployeeID { get; set; }
        public string Department { get; set; }
        public string AssignedDivision { get; set; }
        public string AssignedFactory { get; set; }
        public string AssignedEmployeeID { get; set; }
        public string AssignedDepartment { get; set; }
        public string PermissionGroup { get; set; } //new
        public string EmploymentStatus { get; set; }
        public string PerformanceAssessmentResponsibilityDivision { get; set; }
        public string IdentityType { get; set; } //new
        // public string SupervisorType { get; set; } //new
        // public List<DepartmentSupervisorList> DepartmentSupervisorList { get; set; } //new
        public string LocalFullName { get; set; }
        public string PreferredEnglishFullName { get; set; } //new
        public string ChineseName { get; set; } //new
        public string PassportFullName { get; set; } //new
        public string Gender { get; set; } //new
        public string BloodType { get; set; } //new
        public string MaritalStatus { get; set; } //new
        public DateTime DateOfBirth { get; set; } //new
        public string DateOfBirthStr { get; set; } //new
        public string PhoneNumber { get; set; } //new
        public string MobilePhoneNumber { get; set; } //new
        public string Education { get; set; } //new
        public string Religion { get; set; } //new
        public string TransportationMethod { get; set; } //new
        public string VehicleType { get; set; } //new
        public string LicensePlateNumber { get; set; } //new
        public int? NumberOfDependents { get; set; } //new
        public string RegisteredProvinceDirectly { get; set; } //new
        public string RegisteredCity { get; set; } //new
        public string RegisteredAddress { get; set; } //new
        public string MailingProvinceDirectly { get; set; } //new
        public string MailingCity { get; set; } //new
        public string MailingAddress { get; set; } //new
        public string WorkShiftType { get; set; } //new
        public bool SwipeCardOption { get; set; } //new
        public string SwipeCardNumber { get; set; } //new
        public decimal PositionGrade { get; set; } //new
        public string PositionTitle { get; set; } //new
        public string WorkType { get; set; } //new
        public string Restaurant { get; set; } //new
        // public string SalaryType { get; set; } //new
        // public string SalaryPaymentMethod { get; set; } //new
        public string WorkLocation { get; set; } //new
        public bool? UnionMembership { get; set; } //new
        public DateTime OnboardDate { get; set; }
        public string OnboardDateStr { get; set; }
        public DateTime DateOfGroupEmployment { get; set; }
        public string DateOfGroupEmploymentStr { get; set; }
        public DateTime SeniorityStartDate { get; set; }
        public string SeniorityStartDateStr { get; set; }
        public DateTime AnnualLeaveSeniorityStartDate { get; set; } //new
        public string AnnualLeaveSeniorityStartDateStr { get; set; }
        public DateTime? DateOfResignation { get; set; }
        public string DateOfResignationStr { get; set; }
        public string ReasonResignation { get; set; } //new
        public bool? Blacklist { get; set; } //new
        public DateTime UpdateTime { get; set; } //new
        public string UpdateBy { get; set; } //new
    }

    public class ExitEmployeeMasterFileHistoricalDataView
    {
        public string USER_GUID { get; set; }
        public string Nationality { get; set; }
        public string IdentificationNumber { get; set; }
        public string LocalFullName { get; set; }
        public string Division { get; set; }
        public string Factory { get; set; }
        public string EmployeeID { get; set; }
        public string Department { get; set; }
        public string OnboardDate { get; set; }
        public string GroupDate { get; set; }
        public string ResignDate { get; set; }
        public string DeletionCode { get; set; }
        public string EmploymentStatus { get; set; }
    }

    public class ExitEmployeeMasterFileHistoricalDataParam
    {
        private string _identificationNumber;
        private string _employeeID;
        private string _localFullName;
        public string Nationality { get; set; }
        public string IdentificationNumber { get => _identificationNumber; set => _identificationNumber = value?.Trim(); }
        public string Division { get; set; }
        public string Factory { get; set; }
        public string EmployeeID { get => _employeeID; set => _employeeID = value?.Trim(); }
        public string LocalFullName { get => _localFullName; set => _localFullName = value?.Trim().ToLower(); }
        public string OnboardDateStart { get; set; }
        public string OnboardDateEnd { get; set; }
        public string OnboardDateStart_Str { get; set; }
        public string OnboardDateEnd_Str { get; set; }
        public string GroupDateStart { get; set; }
        public string GroupDateEnd { get; set; }
        public string GroupDateStart_Str { get; set; }
        public string GroupDateEnd_Str { get; set; }
        public string ResignDateStart { get; set; }
        public string ResignDateEnd { get; set; }
        public string ResignDateStart_Str { get; set; }
        public string ResignDateEnd_Str { get; set; }
        public string Language { get; set; }
    }
}