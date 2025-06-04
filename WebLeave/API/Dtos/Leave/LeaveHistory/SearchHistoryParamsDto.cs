namespace API.Dtos.Leave.LeaveHistory
{
    public class SearchHistoryParamsDto
    {
        public int UserID { get; set; }
        public string EmpId { get; set; }
        public int Status { get; set; }
        public int CategoryId { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Lang { get; set; }
    }
    public class HistoryExportParam : SearchHistoryParamsDto
    {
        public string PartName { get; set; }
        public string DeptName { get; set; }
        public string Employee { get; set; }
        public string NumberId { get; set; }
        public string Category { get; set; }
        public string TimeStart { get; set; }
        public string TimeEnd { get; set; }
        public string LeaveDay { get; set; }
        public string status1 { get; set; }
        public string Update { get; set; }
       
    }
}