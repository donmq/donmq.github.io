namespace API.DTOs.AttendanceMaintenance
{
    public class GenerationActiveParam
    {
        public string Factory { get; set; }
        public string Department { get; set; }

        /// <summary>
        /// Year-Month of Attendance - Ngày tháng tham dự
        /// </summary>
        /// <value></value>
        public string Att_Month { get; set; }
        
        public string Deadline_Start { get; set; }
        public string Deadline_End { get; set; }
        public decimal Working_Days { get; set; }
        public string Employee_ID_Start { get; set; }
        public string Employee_ID_End { get; set; }
        public bool Is_Delete { get; set; }
        public string UserName { get; set; }
        public DateTime Current { get; set; }
    }

    public class SearchAlreadyDeadlineDataParam
    {
        public string Factory { get; set; }
        public string Att_Month_Start { get; set; }
        public string Att_Month_End { get; set; }
    }

    public class SearchAlreadyDeadlineDataMain
    {
        public string Factory { get; set; }
        public string Att_Month { get; set; }
        public string Deadline_Start { get; set; }
        public string Deadline_End { get; set; }
        public string Update_By { get; set; }
        public string Update_Time { get; set; }
    }

    public class MonthlyAttendanceDataGenerationActiveEmployees_MonthlyDataCloseParam
    {
        public string Factory { get; set; }
        public string Att_Month { get; set; }
        public string Pass { get; set; }
    }
}