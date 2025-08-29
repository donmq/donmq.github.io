using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs.AttendanceMaintenance
{
    public class IndividualMonthlyWorkingHoursReportDto
    {
        public IndividualMonthlyWorkingHoursReportParam param { get; set; }
        public string print_By { get; set; }
        public string print_Date { get => DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"); }
        public List<ExcelColumn_5_2_16> DataExcels { get; set; }
        
    }

    public class ExcelColumn_5_2_16
    { 
        public string department { get; set; }
        public string department_Name { get; set; }
         public string Employee_ID { get; set; }
         public string Local_Full_Name { get; set; }
        public decimal? normal_Working_Hours { get; set; }
        public decimal? overtime_Hour { get; set; }
        public decimal? total_Working_Hours { get; set; }
    }

    public class IndividualMonthlyWorkingHoursReportParam
    {
        public string factory { get; set; }
        public string language { get; set; }
        public string yearMonth { get; set; }
        public string permission_Group { get; set; }  
    }

    public class PersonalInfo_5_43
    {
        public string Factory { get; set; }
        public string Employee_ID { get; set; }
        public string Department { get; set; }
        public string Local_Full_Name { get; set; }
        public string Work_Shift_Type { get; set; } 
        public bool Swipe_Card_Option { get; set; } 
        public DateTime Onboard_Date { get; set; } 
    }
}