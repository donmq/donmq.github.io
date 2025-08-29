
using API.Models;

namespace API.DTOs.AttendanceMaintenance
{
    public partial class MaintenanceActiveEmployeesDto : HRMS_Att_Monthly
    {
        public bool isProbation { get; set; }
    }
    public partial class MaintenanceActiveEmployeesDto_Detail : HRMS_Att_Monthly_Detail
    {
        public bool isProbation { get; set; }
    }
    public class MaintenanceActiveEmployeesMain
    {
        public string Division { get; set; }
        public string Factory { get; set; }
        public string Att_Month { get; set; }
        public string Department { get; set; }
        public string Employee_ID { get; set; }
        public string Local_Full_Name { get; set; }
        public string Pass { get; set; }
        public string Resign_Status { get; set; }
        public string Permission_Group { get; set; }
        public string Salary_Type { get; set; }
        public string Salary_Days { get; set; }
        public string Actual_Days { get; set; }
        public string Update_By { get; set; }
        public string Update_Time { get; set; }
        public string Probation { get; set; }
        public bool isProbation { get; set; }
    }

    public class MaintenanceActiveEmployeesParam
    {
        public string Factory { get; set; }
        public string Att_Month_Start { get; set; }
        public string Att_Month_End { get; set; }
        public string Department { get; set; }
        public string Employee_ID { get; set; }
        public string Salary_Days { get; set; }
        public string Language { get; set; }

    }

    public class ActiveEmployeeParam
    {
        public string Factory { get; set; }
        public string Employee_ID { get; set; }
        public string Att_Month { get; set; }
        public string Language { get; set; }
        public bool isAdd {get;set;}
        public bool isMonthly {get;set;}
    }

    public class EmpInfo522
    {
        public string USER_GUID { get; set; }
        public string Department { get; set; }
        public string Department_Code { get; set; }
        public string Local_Full_Name { get; set; }
        public string Division { get; set; }
        public string Permission_Group { get; set; }
    }

    public class MaintenanceActiveEmployeesDetail
    {
        public string USER_GUID { get; set; }
        public string Division { get; set; }
        public string Factory { get; set; }
        public string Att_Month { get; set; }
        public string Att_Month_Str { get; set; }
        public string Deadline_Start { get; set; }
        public string Deadline_Start_Str { get; set; }
        public string Deadline_End { get; set; }
        public string Deadline_End_Str { get; set; }
        public string Pass { get; set; }
        public string Employee_ID { get; set; }
        public string Local_Full_Name { get; set; }
        public string Department { get; set; }
        public string Department_Code { get; set; }
        public string Department_Name { get; set; }
        public decimal Salary_Days { get; set; }
        public decimal Actual_Days { get; set; }
        public string Permission_Group { get; set; }
        public string Salary_Type { get; set; }
        public string Resign_Status { get; set; }
        public int Delay_Early { get; set; }
        public int No_Swip_Card { get; set; }

        /// <summary>
        /// Overtime Meal  Times
        /// </summary>
        public int? DayShift_Food { get; set; }

        /// <summary>
        /// Night Shift Meal Times
        /// </summary>
        public int? NightShift_Food { get; set; }

        /// <summary>
        /// Overtime Meal Times
        /// </summary>
        public int Food_Expenses { get; set; }
        
        public int Night_Eat_Times { get; set; }
        public string Probation { get; set; }
        public bool isProbation { get; set; }
        public List<LeaveAllowance> Leaves { get; set; }
        public List<LeaveAllowance> Allowances { get; set; }
        public List<decimal?> LeaveCodes { get; set; } = new();
        public List<decimal?> AllowanceCodes { get; set; } = new();
    }

    public class LeaveAllowance
    {
        public string Code { get; set; }
        public string CodeName { get; set; }
        public string Days { get; set; }
        public string Total { get; set; }
    }

    public class MaintenanceActiveEmployeesDetailParam
    {
        public string Factory { get; set; }
        public string Att_Month { get; set; }
        public string Employee_ID { get; set; }
        public string Language { get; set; }
        public bool isProbation { get; set; }
    }

    public class Upd_HRMS_Att_Yearly_Param
    {
        public string Factory { get; set; }
        public DateTime Att_Year { get; set; }
        public string Employee_ID { get; set; }
        public string USER_GUID { get; set; }
        public string Leave_Type { get; set; }
        public decimal Days { get; set; }
        public string Account { get; set; }
        public List<LeaveAllowance> Details { get; set; }
    }
}