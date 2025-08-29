using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs.AttendanceMaintenance
{
    public class OvertimeHoursReportParam
    {
        public string Factory { get; set; }
        public string Department { get; set; }
        public string Employee_Id { get; set; }
        public string Work_Shift_Type { get; set; }
        public string Kind { get; set; }
        public List<string> Permission_Group { get; set; }
        public string Date_From { get; set; }

        public string Date_To { get; set; }

        public string Lang { get; set; }

    }
    public class OvertimeHoursReport
    {
        public string Factory { get; set; }
        public string Department { get; set; }
        public string Employee_Id { get; set; }
        public string Work_Shift_Type { get; set; }
        public string Kind { get; set; }
        public string Permission_Group { get; set; }
        public string Department_Name { get; set; }
        public string Local_Full_Name { get; set; }
        public decimal TrainingHours { get; set; }
        public decimal RegularOvertime { get; set; }
        public decimal HolidayOvertime { get; set; }
        public decimal NightHours { get; set; }
        public decimal NightOvertimeHour { get; set; }
        public string PositionTitle { get; set; }
        public decimal OvertimeHours { get; set; }
        public decimal WorkingHours { get; set; }
    }
}