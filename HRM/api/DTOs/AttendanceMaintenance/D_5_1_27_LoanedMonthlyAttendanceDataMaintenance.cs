namespace API.DTOs.AttendanceMaintenance
{
    public class LoanedMonthlyAttendanceDataMaintenanceParam
    {
        public string Factory { get; set; }
        public string Att_Month_From { get; set; }
        public string Att_Month_To { get; set; }
        public string Department { get; set; }
        public string Employee_ID { get; set; }
        public string Salary_Days { get; set; }
        public string Lang { get; set; }
    }

    public class LoanedMonthlyAttendanceDataMaintenanceDto
    {
        public string USER_GUID { get; set; }
        public string Division { get; set; }
        public bool Pass { get; set; }
        public string Pass_Str { get; set; }
        public string Factory { get; set; }
        public string Att_Month { get; set; }
        public string Department { get; set; }
        public string Department_Code { get; set; }
        public string Department_Name { get; set; }
        public string Employee_ID { get; set; }
        public string Local_Full_Name { get; set; }
        public string Resign_Status { get; set; }
        public int Delay_Early { get; set; }
        public int No_Swip_Card { get; set; }
        public int Food_Expenses { get; set; }
        public int Night_Eat_Times { get; set; }
        public string Permission_Group { get; set; }
        public string Salary_Type { get; set; }
        public decimal Salary_Days { get; set; }
        public decimal Actual_Days { get; set; }
        public string Update_By { get; set; }
        public string Update_Time { get; set; }
        public string Lang { get; set; }
        public List<DetailDisplay> Leaves { get; set; }
        public List<DetailDisplay> Allowances { get; set; }
        public List<decimal?> Leave_Days { get; set; } = new();
        public List<decimal?> Allowance_Days { get; set; } = new();
    }

    public class DetailParam
    {
        public string Factory { get; set; }
        public string Att_Month { get; set; }
        public string Employee_ID { get; set; }
        public string USER_GUID { get; set; }
        public string Lang { get; set; }
    }

    public class Detail
    {
        public List<DetailDisplay> Leaves { get; set; }
        public List<DetailDisplay> Allowances { get; set; }
    }

    public class DetailDisplay
    {
        public string Code { get; set; }
        public string CodeName { get; set; }
        public decimal Days { get; set; }
        public decimal Total { get; set; }
    }

    public class EmployeeInfo
    {
        public string USER_GUID { get; set; }
        public string Division { get; set; }
        public string Local_Full_Name { get; set; }
        public string Permission_Group { get; set; }
        public string Department { get; set; }
        public string Salary_Type { get; set; }
        public List<DetailDisplay> Leaves { get; set; }
        public List<DetailDisplay> Allowances { get; set; }
    }

    public class YearlyUpdate
    {
        public string Factory { get; set; }
        public DateTime Att_Year { get; set; }
        public string Employee_ID { get; set; }
        public string USER_GUID { get; set; }
        public string Leave_Type { get; set; }
        public string Account { get; set; }
        public List<DetailDisplay> Details { get; set; }
    }
}