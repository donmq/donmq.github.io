namespace API.DTOs.OrganizationManagement
{
    public class HRMS_Org_DepartmentDto
    {
        public string Division { get; set; }
        public string Factory { get; set; }
        public string Center_Code { get; set; }
        public string Org_Level { get; set; }
        public string Org_Level_Name { get; set; }
        public string Department_Code { get; set; }
        public string Department_Name { get; set; }
        public string Department_Name_Lang { get; set; }
        public string Upper_Department { get; set; }
        public string Upper_Department_Name { get; set; }
        public string Attribute { get; set; }
        public string Virtual_Department { get; set; }
        public string Virtual_Department_Name { get; set; }
        public bool IsActive { get; set; }
        public string Supervisor_Employee_ID { get; set; }
        public string Supervisor_Type { get; set; }
        public int? Approved_Headcount { get; set; }
        public string Cost_Center { get; set; }
        public DateTime Effective_Date { get; set; }
        public DateTime? Expiration_Date { get; set; }
        public string Update_By { get; set; }
        public DateTime Update_Time { get; set; }
        public string status { get; set; }
    }
    public class HRMS_Org_Department_Param
    {
        public string Status { get; set; }
        public string Division { get; set; }
        public string Factory { get; set; }
        public string Department_Code { get; set; }
        public string Department_Name { get; set; }
        public string lang { get; set; }
    }
    public class LanguageDeparment
    {
        public string Department_Code { get; set; }
        public string Division { get; set; }
        public string Factory { get; set; }
        public List<LanguageParam> Detail { get; set; }
        public string userName { get; set; }
    }

    public class LanguageParam
    {
        public string Language_Code { get; set; }
        public string Department_Name { get; set; }
    }
    public class ListUpperVirtual
    {
        public string Department { get; set; }
        public string DepartmentName { get; set; }

    }
}