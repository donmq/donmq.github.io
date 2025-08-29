namespace API.DTOs.AttendanceMaintenance
{
    public class WorkingHoursReportParam
    {
        public string Factory { get; set; }
        public string Department { get; set; }
        public string Date_From { get; set; }
        public string Date_To { get; set; }
        public int Salary_WorkDays { get; set; }
        public string Lang { get; set; }
    }
    public class WorkingHoursReportDto
    {
        public string Department { get; set; }
        public string Department_Name { get; set; }
        public string Employee_ID { get; set; }
        public string Local_Full_Name { get; set; }
        public string Onboard_Date { get; set; }
        public string Resign_Date { get; set; }
        public decimal Actual_Work_Days { get; set; }
        public int Night_Shift_Allowance_Times { get; set; }
        public List<string> Leave_List { get; set; }
        public List<string> Allowance_List { get; set; }
        public List<decimal> Leave_Days { get; set; }
        public List<decimal> Allowance_Days { get; set; }
        public string Lang { get; set; }
    }

    public class CodeDto
    {
        public int Seq { get; set; }
        public string Code { get; set; }
        public string Char1 { get; set; }
        public string Char2 { get; set; }
        public string CodeName { get; set; }
        public string DisplayName => $"{Code} - {CodeName}";
    }

    public class WorkingHoursHeader
    {
        public List<string> Headers { get; set; }
        public int RowIndex { get; set; }

        public WorkingHoursHeader(List<string> headers, int rowIndex)
        {
            Headers = headers;
            RowIndex = rowIndex;
        }
    }

    /// <summary>
    /// Lấy danh sách Header Theo Ngôn ngữ
    /// </summary>
    public class WorkingHoursHeaderMultipleLanguage
    {
        public List<string> HeadersEN { get; set; }
        public List<string> HeadersTW { get; set; }

        public WorkingHoursHeaderMultipleLanguage() { }
        public WorkingHoursHeaderMultipleLanguage(List<string> headersEN, List<string> headersTW)
        {
            HeadersEN = headersEN;
            HeadersTW = headersTW;
        }
    }
}