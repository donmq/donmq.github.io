using API.Models;

namespace API.DTOs.AttendanceMaintenance
{
     public partial class ResignedEmployeeDto : HRMS_Att_Resign_Monthly
    {
        public bool isProbation { get; set; }
    }
    public partial class ResignedEmployeeDto_Detail : HRMS_Att_Resign_Monthly_Detail
    {
        public bool isProbation { get; set; }
    }
    public class ResignedEmployeeMain
    {
        public string USER_GUID { get; set; }
        public string Pass { get; set; }
        public string Factory { get; set; }
        public string Att_Month { get; set; }
        public string Department { get; set; }
        public string Department_Code { get; set; }
        public string Department_Name { get; set; }
        public string Employee_ID { get; set; }
        public string Local_Full_Name { get; set; }
        public string Resign_Status { get; set; }
        public string Permission_Group { get; set; }
        public string Permission_Group_Name { get; set; }
        public string Salary_Type { get; set; }
        public string Salary_Type_Name { get; set; }
        public decimal Salary_Days { get; set; }
        public decimal Actual_Days { get; set; }
        public string Update_By { get; set; }
        public string Update_Time { get; set; }
        public string Probation { get; set; }
        public bool isProbation { get; set; }
    }

    public class ResignedEmployeeDetail : ResignedEmployeeMain
    {
        public string Division { get; set; }
        public int Delay_Early { get; set; }
        public int No_Swip_Card { get; set; }
        public int Food_Expenses { get; set; }
        public int Night_Eat_Times { get; set; }
        
        /// <summary>
        /// Số ngày ăn trưa
        /// </summary>
        /// <value></value>
        public int? DayShift_Food { get; set; }
        
        /// <summary>
        /// Số ngày ăn khuya
        /// </summary>
        /// <value></value>
        public int? NightShift_Food { get; set; }

        public List<LeaveDetailDisplay> Leaves { get; set; }
        public List<LeaveDetailDisplay> Allowances { get; set; }
        public List<decimal?> LeaveCodes { get; set; } = new();
        public List<decimal?> AllowanceCodes { get; set; } = new();
    }

    public class LeaveDetailDisplay
    {
        public string Code { get; set; }
        public string CodeName { get; set; }
        public string Days { get; set; }
        public string Total { get; set; }
    }

    public class EmpResignedInfo
    {
        public string USER_GUID { get; set; }
        public string Local_Full_Name { get; set; }
        public string Department_Name { get; set; }
        public string Department_Code {get; set;}
        public string Division { get; set; }
        public string Permission_Group { get; set; }
    }

    public class ResignedEmployeeParam
    {
        public string Factory { get; set; }
        public string Att_Month_Start { get; set; }
        public string Att_Month_End { get; set; }
        public string Department { get; set; }
        public string Employee_ID { get; set; }
        public string PrintBy { get; set; }
        public decimal? Salary_Days { get; set; }
        public string Language { get; set; }
        public bool isProbation { get; set; }
        public bool isMonthly {get; set;}
    }

    public class ResignedEmployeeDetailParam
    {
        public string Factory { get; set; }
        public string Att_Month { get; set; }
        public string Employee_ID { get; set; }
        public string USER_GUID { get; set; }
        public string Language { get; set; }
    }

    public class AttYearlyUpdate
    {
        public string Factory { get; set; }
        public DateTime Att_Year { get; set; }
        public string Employee_ID { get; set; }
        public string USER_GUID { get; set; }
        public string Leave_Type { get; set; }
        public string Account { get; set; }
        public List<LeaveDetailDisplay> Details { get; set; }
    }
}