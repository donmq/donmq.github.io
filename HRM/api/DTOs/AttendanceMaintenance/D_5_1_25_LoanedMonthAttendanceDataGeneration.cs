
namespace API.DTOs.AttendanceMaintenance
{
    public class LoanedDataGenerationDto
    {
        public string Factory { get; set; }
        public string Loaned_Year_Month_Str { get; set; }
        public string Loaned_Date_Start_Str { get; set; }
        public string Loaned_Date_End_Str { get; set; }
        public decimal Normal_Working_Days { get; set; }
        public string Employee_ID_Start { get; set; }
        public string Employee_ID_End { get; set; }
        public string Selected_Tab { get; set; }
        public string Close_Status { get; set; }
        public string Language { get; set; }
        public string UserName { get; set; }
    }

    public class QuerySumDayParam
    {
        public string Factory { get; set; }
        public string Employee_ID { get; set; }
        public string Att_Month { get; set; }
        public string Leave_Type { get; set; } = "1";
        public List<string> LeaveCodes { get; set; } = new()
        {
            "A0", "J3", "B0", "C0", "I0", "I1", "N0", "D0", "E0", "F0",
            "G0", "G2", "G1", "H0", "K0", "J0", "J2", "J5", "J1", "O0", "J4"
        };
        public decimal Normal_Working_Days { get; set; }
    }

    public class QueryMainSumDayParam
    {
        public string Factory { get; set; }
        public string Employee_ID { get; set; }
        public string Leave_Date_Start { get; set; }
        public string Leave_Date_End { get; set; }
        public string Leave_Code { get; set; }
    }

    public class QueryMainSumHoursParam
    {
        public string Factory { get; set; }
        public string Employee_ID { get; set; }
        public string Overtime_Date_Start { get; set; }
        public string Overtime_Date_End { get; set; }
        public string Holiday_Code { get; set; }
        public string Overtime_Column_Name { get; set; }
    }

    public class MainSumHours
    {
        public string Overtime_Code { get; set; }
        public string Holiday_Code { get; set; }
        public string Overtime_Column_Name { get; set; }
    }
}