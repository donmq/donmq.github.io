namespace API.Dtos.Report.ReportShow
{
    public class ExportExcelDateDto
    {
        public string EmployeeNumber { get; set; }
        public string No { get; set; }
        public string EmployeeName { get; set; }
        public string PartCode { get; set; }
        public string EmployeePostition { get; set; }
        public string LeaveType { get; set; }
        public string Time_Start { get; set; }
        public string Time_Of_Leave { get; set; }
        public string Status { get; set; }
        public string FromDate { get; set; }
        public string EndDate { get; set; }
        public ReportDateDetailDTO ReportDateDetailDTO { get; set; }
    }
}