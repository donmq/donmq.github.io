using API.Models;

namespace API.DTOs.AttendanceMaintenance
{
    public class MonthlyEmployeeStatusChangesSheet_ByGender_Param
    {
        public string Factory { get; set; }
        public string YearMonth { get; set; }
        public string FirstDate { get; set; }
        public string LastDate { get; set; }
        public int Level { get; set; }
        public string LevelName { get; set; }
        public List<string> PermissionGroup { get; set; }
        public List<string> PermissionName { get; set; }
        public string Lang { get; set; }
    }

    public class MonthlyEmployeeStatusChangesSheet_ByGender_Data
    {
        public List<string> Employee_Dept { get; set; }
        public List<HRMS_Emp_Personal> HEP { get; set; }
        public List<HRMS_Emp_IDcard_EmpID_History> HEIEH { get; set; }
        public List<HRMS_Org_Department> HOD { get; set; }
        public List<HRMS_Org_Department_Language> HODL { get; set; }
        public List<string> Foreign_Permission_list { get; set; }
        public string currentDepartment { get; set; }
    }

    public class MonthlyEmployeeStatusChangesSheet_ByGender_Report
    {
        public string Department { get; set; }
        public string Department_Name { get; set; }
        public int Total_Headcount { get; set; }
        public int Male { get; set; }
        public int Female { get; set; }
        public string Turnover_Rate { get; set; }
    }
    public class ParentDepartment_ByGender
    {
        public string Parent_Department { get; set; }
        public string Parent_Department_Name { get; set; }
        public string Parent_Department_Level { get; set; }
    }
    public class MonthlyEmployeeStatusChangesSheet_ByGender_Departments : ParentDepartment_ByGender
    {
        public string Employee_Dept { get; set; }
        public string DepartmentName { get; set; }
        public string OrgLevel { get; set; }

        public int Begin_Cnt { get; set; }
        public int NewHires_Cnt { get; set; }
        public int Resign_Cnt { get; set; }
        public int Male_Cnt { get; set; }
        public int Female_Cnt { get; set; }
    }
}