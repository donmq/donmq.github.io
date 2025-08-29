namespace API.DTOs.AttendanceMaintenance
{

    public class GenerationResignedParam
    {
        public string Factory { get; set; }
        public string Att_Month { get; set; }
        public string Resign_Date { get; set; }
        public string Employee_ID_Start { get; set; }
        public string Employee_ID_End { get; set; }
        public string UserName { get; set; }
        public DateTime Current { get; set; }
    }
    public class GenerationResigned : GenerationResignedParam
    {
        public decimal Working_Days { get; set; }
        public bool Is_Delete { get; set; }
    }
    public class MonthlyAttendanceDataGenerationResignedEmployees_MonthlyDataCloseParam
    {
        public string Factory { get; set; }
        public string Att_Month { get; set; }
        public string Pass { get; set; }
    }
}