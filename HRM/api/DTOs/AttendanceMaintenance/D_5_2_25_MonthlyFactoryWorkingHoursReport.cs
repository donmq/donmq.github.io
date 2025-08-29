using API.Models;

namespace API.DTOs.AttendanceMaintenance
{
    public class MonthlyFactoryWorkingHoursReportPT : HRMS_Emp_Personal
    {
        public decimal nor_hr { get; set; } = 0m;
        public decimal ovhrd { get; set; } = 0m;
        public decimal publ { get; set; } = 0m;
        public decimal stp { get; set; } = 0m;
    }
    public class MonthlyFactoryWorkingHoursReportDto
    {
        public string Dept_Kind { get; set; }
        public string Flag { get; set; }
        public string Employee_ID { get; set; }
        public string Department { get; set; }
        public string Department_Name { get; set; }
        public string Local_Full_Name { get; set; }
        public string Position_Title { get; set; }
        public string Position_Title_Name { get; set; }
        public string Work_Type { get; set; }
        public string Work_Type_Name { get; set; }
        public DateTime Onboard_Date { get; set; }
        public DateTime? Resign_Date { get; set; }
        public string Fun { get; set; }
        public string Fun_Name { get; set; }
        public string Dept { get; set; }
        public string Dept_Name { get; set; }
        public decimal report_hr { get; set; }
        public decimal actual_hr { get; set; }
        public decimal nor_hr { get; set; } 
        public decimal ovhrd { get; set; } 
        public decimal publ { get; set; } 
        public decimal stp { get; set; }
    }

    public class MonthlyFactoryWorkingHoursReportParam
    {
        public string Factory { get; set; }
        public string Start_Date { get; set; }
        public string End_Date { get; set; }
        public List<string> Permission_Group { get; set; }
        public string Language { get; set; }
        public string UserName { get; set; }
    }
}