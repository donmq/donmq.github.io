namespace API.Dtos.SeaHr.SeaHrHistory
{
    public class SearchHistoryParamsDto
    {
        public string EmpId { get; set; }
        public int Status { get; set; }
        public int CategoryId { get; set; }
        public int DepartmentId { get; set; }
        public List<int> PartList { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Lang { get; set; }
        // title excel
        public string PartCode { get; set; }
        public string DeptName { get; set; }
        public string Employee { get; set; }
        public string NumberID { get; set; }
        public string Category { get; set; }
        public string Created { get; set; }
        public string TimeStart { get; set; }
        public string DateStart { get; set; }
        public string TimeEnd { get; set; }
        public string DateEnd { get; set; }
        public string LeaveDay { get; set; }
        public string SearchDate { get; set; }
        public string LeaveDayByRangerDate { get; set; }
        public string StatusExcel { get; set; }
        public string Update { get; set; }
        public string SEAHRNote { get; set; }
    }
}