
namespace API.DTOs.AttendanceMaintenance
{
    public class DepartmentMonthlyWorkingHoursReportDto
    {
        public DepartmentMonthlyWorkingHoursReportParam param { get; set; }
        public string print_By { get; set; }
        public string print_Date { get => DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"); }
        public List<DataExcel> DataExcels { get; set; }
    }

    public class DataExcel
    { 
        public string department { get; set; }
        public string department_Name { get; set; }
        public int total_Headcount { get; set; }
        public int new_Recruits { get; set; }
        public decimal? normal_Working_Hours { get; set; }
        public decimal? overtime_Hour { get; set; }
        public decimal? total_Working_Hours { get; set; }
        public decimal? average_Total_Working_Hours_Per_Person { get; set; }
    }

    public class DepartmentMonthlyWorkingHoursReportParam
    {
        public string factory { get; set; }
        public string language { get; set; }
        public string yearMonth { get; set; }
        public string permission_Group { get; set; }  
    }

    public class PersonalInfo
    {   
        public string Employee_ID { get; set; }    
        public string Work_Shift_Type { get; set; } 
        public bool Swipe_Card_Option { get; set; } 
        public DateTime Onboard_Date { get; set; } 
    }
}