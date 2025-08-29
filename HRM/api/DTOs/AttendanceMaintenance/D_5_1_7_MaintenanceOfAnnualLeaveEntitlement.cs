using API.Models;

namespace API.DTOs.AttendanceMaintenance
{
    public class MaintenanceOfAnnualLeaveEntitlementDto
    {
        public string USER_GUID { get; set; }
        public string Annual_Start { get; set; }
        public string Annual_End { get; set; }
        public string Factory { get; set; }
        public string Department_Code { get; set; }
        public string Department_Name { get; set; }
        public string Department_Code_Name { get; set; }
        public string Employee_ID { get; set; }
        public string Local_Full_Name { get; set; }
        public string Leave_Code { get; set; }
        public string Leave_Code_Old { get; set; }
        public string Leave_Code_Name { get; set; }
        public string Previous_Hours { get; set; }
        public string Year_Hours { get; set; }
        public decimal Total_Hours { get; set; }
        public decimal Total_Days { get; set; }
        public string Update_By { get; set; }
        public string Update_Time { get; set; }
        public bool IsEdit { get => false; }
        public string Id { get => Guid.NewGuid().ToString(); }
        public bool IsDisabled { get => false; }
    }

    public class EmpLeaveInfo
    {
        public string USER_GUID { get; set; }
        public string Local_Full_Name { get; set; }
        public string Department { get; set; }
    }

    public class MaintenanceOfAnnualLeaveEntitlementParam
    {
        public string Factory { get; set; }
        public string Department { get; set; }
        public string Employee_ID { get; set; }
        public string Leave_Code { get; set; }
        public string AvailableRange_Start { get; set; }
        public string AvailableRange_End { get; set; }
        public string Language { get; set; }
    }

    public class AnnualLeaveDetailParam
    {
        public string Factory { get; set; }
        public string AvailableRange_Start { get; set; }
        public string Employee_ID { get; set; }
        public string Leave_Code { get; set; }
        public string Language { get; set; }
    }

    public class DepartmentMain
    {
        public string Division { get; set; }
        public string Factory { get; set; }
        public string Department_Code { get; set; }
        public string Department_Name { get; set; }
        public string Department_Name_Str { get; set; }
    }

    public class DepartmentJoinResult
    {
        public HRMS_Org_Department Department { get; set; }
        public HRMS_Org_Department_Language Language { get; set; }
    }

    public class UploadFormData
    {
        public IFormFile File { get; set; }
        public string Language { get; set; }
        public List<string> UserRoles { get; set; }
        public string UserName { get; set; }
    }

    public class UploadReport
    {
        public string Factory { get; set; }
        public string Employee_ID { get; set; }
        public string Leave_Code { get; set; }
        public object AvailableRange_Start { get; set; }
        public object AvailableRange_End { get; set; }
        public object Previous_Hours { get; set; }
        public object Year_Hours { get; set; }
        public string Status { get; set; }
        public string Error { get; set; }
    }
}