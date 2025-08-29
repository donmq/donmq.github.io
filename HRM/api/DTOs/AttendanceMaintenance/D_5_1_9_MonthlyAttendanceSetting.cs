using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;

namespace API.DTOs.AttendanceMaintenance
{
    public class MonthlyAttendanceSettingParam
    {
        public string Factory { get; set; }
        public string Effective_Month { get; set; }
        public string Effective_Month_Str { get; set; }
    }

    public class HRMS_Att_Use_Monthly_LeaveDto
    {       
        public string Factory { get; set; }     
        public DateTime Effective_Month { get; set; }     
        public string Effective_Month_Str { get; set; }     
        public string Leave_Type { get; set; }       
        public string Code { get; set; }       
        public int Seq { get; set; }       
        public bool Month_Total { get; set; }
        public bool? Year_Total { get; set; }     
        public string Update_By { get; set; }       
        public DateTime? Update_Time { get; set; }
        public bool Is_Function_Edit { get; set; }
        public bool Is_Delete { get; set; }
    }
}