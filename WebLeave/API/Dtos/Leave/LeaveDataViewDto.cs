namespace API.Dtos.Leave
{

    public class LeaveDataViewModel : LeaveDataViewDto
    {
        public string EmpName { get; set; }
        public string CategoryNameVN { get; set; }
        public string CategoryNameEN { get; set; }
        public string CategoryNameTW { get; set; }
        public bool? Exhibit { get; set; }
        public bool Checked { get; set; }
        public string Status { get; set; }
        public bool Disable { get; set; }
    }

    public class LeaveDataViewDto
    {
        public int LeaveID { get; set; }
        public string LeaveArchive { get; set; }
        public int? EmpID { get; set; }
        public int? CateID { get; set; }
        public DateTime? Time_Start { get; set; }
        public DateTime? Time_End { get; set; }
        public DateTime? DateLeave { get; set; }
        public double? LeaveDay { get; set; }
        public int? Approved { get; set; }
        public DateTime? Time_Applied { get; set; }
        public string TimeLine { get; set; }
        public string Comment { get; set; }
        public bool? LeavePlus { get; set; }
        public bool? LeaveArrange { get; set; }
        public int? UserID { get; set; }
        public int? ApprovedBy { get; set; }
        public int? EditRequest { get; set; }
        public bool? Status_Line { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Updated { get; set; }
        public bool? Status_Lock { get; set; }
        public string MailContent_Lock { get; set; }
        public string CommentArchive { get; set; }
        public string LeaveDayReturn { get; set; }
        public string ReasonAdjust { get; set; }
    }
}