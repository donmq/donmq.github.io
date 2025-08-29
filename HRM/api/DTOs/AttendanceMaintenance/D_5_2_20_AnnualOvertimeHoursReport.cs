namespace API.DTOs.AttendanceMaintenance
{
    public class AnnualOvertimeHoursReportDto
    {
        public string Department_Code { get; set; }
        public string Department_Name { get; set; }
        public string Employee_ID { get; set; }
        public string Local_Full_Name { get; set; }
        public string Onboard_Date { get; set; }
        public List<decimal> Monthly { get; set; } = new();
        // public decimal? January { get; set; }
        // public decimal? February { get; set; }
        // public decimal? March { get; set; }
        // public decimal? April { get; set; }
        // public decimal? May { get; set; }
        // public decimal? June { get; set; }
        // public decimal? July { get; set; }
        // public decimal? August { get; set; }
        // public decimal? September { get; set; }
        // public decimal? October { get; set; }
        // public decimal? November { get; set; }
        // public decimal? December { get; set; }
        public decimal? Total { get; set; }
    }

    public class AnnualOvertimeHoursReportParam
    {
        public string Factory { get; set; }
        public string Year_Month { get; set; }
        public string Department { get; set; }
        public string Employee_ID { get; set; }
        public string UserName { get; set; }
        public string Language { get; set; }
    }
}