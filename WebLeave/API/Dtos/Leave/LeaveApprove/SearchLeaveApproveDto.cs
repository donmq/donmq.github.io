namespace API.Dtos.Leave.LeaveApprove
{
    public class SearchLeaveApproveDto
    {
        public string EmpNumber { get; set; }
        public int CategoryId { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Lang { get; set; }
        public bool OnView { get; set; }
        public bool IsSearch { get; set; }
        public int UserCurrent { get; set; }
        public string Label_Employee { get; set; }
        public string Label_Fullname { get; set; }
        public int LeaveID { get; set; }
    }
}