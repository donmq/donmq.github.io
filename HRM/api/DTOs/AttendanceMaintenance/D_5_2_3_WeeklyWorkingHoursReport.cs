using API.Models;

namespace API.DTOs.AttendanceMaintenance
{
    public class WeeklyWorkingHoursReportExcel
    {
        public string Department { get; set; }
        public string Department_Name { get; set; }
        public string Employee_ID { get; set; }
        public string Local_Full_Name { get; set; }
        public int Department_Headcount { get; set; }
        public int Hours_0_48 { get; set; }
        public int Hours_48_60 { get; set; }
        public int Hours_60_64 { get; set; }
        public int Hours_64_70 { get; set; }
        public int Hours_70 { get; set; }
    }
    public class ALLReportExcel
    {
        public List<WeeklyWorkingHoursReportExcel> listReport { get; set; }
        public WeeklyWorkingHoursReportExcel Report { get; set; }
    }
    public class WeeklyWorkingHoursReportParam
    {
        public string Factory { get; set; }
        public string Department { get; set; }
        public string Kind { get; set; }
        public DateTime Date_Start { get; set; }
        public DateTime Date_End { get; set; }
        public string Level { get; set; }
        public string language { get; set; }
    }

    public class ListDepartmentDto
    {
        public string Department_Code { get; set; }
        public string Department_Name { get; set; }
        public string Org_Level { get; set; }
    }
    public class TableData
    {
        public List<HRMS_Emp_Personal> dataHEP { get; set; }
        public List<HRMS_Org_Department> dataHOD { get; set; }
        public List<HRMS_Att_Monthly> dataHAM { get; set; }
        public List<HRMS_Att_Work_Shift> dataHAWS { get; set; }
        public List<HRMS_Att_Change_Record> dataHACR { get; set; }
        public List<HRMS_Att_Leave_Maintain> dataHALM { get; set; }
        public List<HRMS_Att_Overtime_Maintain> dataHAOM { get; set; }
    }
    public class EmployeeHoursDetail
    {
        public string Employee_ID { get; set; }
        public string Local_Full_Name { get; set; }
        public decimal Total { get; set; }
    }
}