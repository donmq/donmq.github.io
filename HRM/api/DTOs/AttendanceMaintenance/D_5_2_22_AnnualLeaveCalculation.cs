
using API.Models;

namespace API.DTOs.AttendanceMaintenance
{
    public class AnnualLeaveCalculationDto
    {
        public string Department_Code { get; set; }
        public string Department_Name { get; set; }
        public string Employee_ID { get; set; }
        public string Local_Full_Name { get; set; }
        public string Position_Title { get; set; }
        public string Work_Type { get; set; }
        public string Hazardous_Job { get; set; }
        public string Onboard_Date { get; set; }
        public string Annual_Leave_Seniority_Start_Date { get; set; }
        public string Resign_Date { get; set; }
        public decimal Unused_Annual { get; set; }
        public decimal Used_Annual { get; set; }
        public decimal Used_Annual_Leave_Employee { get; set; }
        public decimal Used_Annual_Leave_Company { get; set; }
        public decimal Seniority_Day { get; set; }
        public decimal Annual_Leave_Days { get; set; }
        public decimal Available_Days { get; set; }
        public int Allocation_Annual_Company { get; set; }
        public decimal Allocation_Annual_Employee { get; set; }
        public List<decimal> YearMonth { get; set; } = new();
    }

    public class AnnualLeaveCalculationParam
    {
        public string Factory { get; set; }
        public string Kind { get; set; }
        public string Department { get; set; }
        public string Start_Year_Month { get; set; }
        public string End_Year_Month { get; set; }
        public List<string> Permission_Group { get; set; }
        public string UserName { get; set; }
        public string Language { get; set; }
    }

    public class Percent_Param
    {
        public List<HRMS_Att_Calendar> HACs { get; set; }
        public List<HRMS_Att_Change_Record> HACRs { get; set; }
        public List<HRMS_Att_Leave_Maintain> HALMs { get; set; }
        public DateTime Start_Date { get; set; }
        public DateTime End_Date { get; set; }
        public string Employee_ID { get; set; }
    }

    public class Query_Available_Leave_Days_Param
    {
        public decimal Total_Annual { get; set; }
        public decimal Sub_Month { get; set; }
        public DateTime Start_Date { get; set; }
        public DateTime End_Date { get; set; }
        public HRMS_Emp_Personal HEP { get; set; }
        public List<HRMS_Att_Calendar> HACs { get; set; }
        public List<HRMS_Att_Change_Record> HACRs { get; set; }
        public List<HRMS_Att_Leave_Maintain> HALMs { get; set; }
    }
}