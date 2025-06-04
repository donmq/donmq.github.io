using API.Models;

namespace API.Dtos.Report.ReportShow
{
    public class ReportShowModelDTO
    {
        public DateTime LeaveDate { get; set; }
        public int DayOfWeek { get; set; }
        public int SEAMP { get; set; }
        public int Applied { get; set; }
        public int Approved { get; set; }
        public int Actual { get; set; }
        public int MPPoolOut { get; set; }
        public int MPPoolIn { get; set; }
        public int Total { get; set; }
        public string Time_Start { get; set; }
        public string Time_End { get; set; }
        public double? LeaveDay { get; set; }
        public double? Hour { get; set; }
        public string LeaveType { get; set; }
        public string PartCode { get; set; }
        public string DeptCode { get; set; }
        public string EmployeeNumber { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeePostition { get; set; }
        public int LeaveStatus { get; set; }
        public string LeaveStatus_Str { get; set; }
        public int Index { get; set; }

        // public string title { get; set; }
        public LeaveData leaveData { get; set; }
    }

    public class ExcelExportReportShowModel
    {
        public int Index { get; set; }
        public string LeaveDate { get; set; }
        public int SEAMP { get; set; }
        public int Applied { get; set; }
        public int Approved { get; set; }
        public int Actual { get; set; }
        public int MPPoolOut { get; set; }
        public int MPPoolIn { get; set; }
        public int Total { get; set; }
    }

}