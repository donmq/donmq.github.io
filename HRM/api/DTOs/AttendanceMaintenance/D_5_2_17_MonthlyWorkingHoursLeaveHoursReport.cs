namespace API.DTOs.AttendanceMaintenance
{
    public class MonthlyWorkingHoursLeaveHoursReportDto
    {
        public string Factory { get; set; }
        public string Att_Month { get; set; }
        public string Employee_ID { get; set; }
        public string YearMonth { get; set; }
        public string PermissionGroup { get; set; }
        public int GS14 { get; set; }
        public decimal? GS17 { get; set; }
        public decimal? GS26 { get; set; }
        public decimal? GS27 { get; set; }
        public decimal? AR1 { get; set; }
        public decimal? AR2 { get; set; }
        public decimal? AR3 { get; set; }
        public decimal? AR4 { get; set; }
        public decimal? ARAb { get; set; }
    }

    public class MonthlyWorkingHoursLeaveHoursReportParam
    {
        public string Factory { get; set; }
        public string YearMonth { get; set; }
        public List<string> PermissionGroup { get; set; }
        public string option { get; set; }
        public string Language { get; set; }
        public string UserName { get; set; }
    }
}