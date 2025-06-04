namespace API.Dtos.SeaHr.SeaHrHistory
{
    public class LeaveDataDto
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
        public string Time_Applied { get; set; }        
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
        //
        public string EmpName { get; set; }
        public string EmpNumber { get; set; }
        public int? DeptID { get; set; }
        public string DeptIDName { get; set; }
        public string DeptName { get; set; }
        public string DeptNameLangZH { get; set; }
        public string DeptNameLangVN { get; set; }
        public string DeptNameLangEN { get; set; }
        public int? PartID { get; set; }
        public string PartName { get; set; }
        public string PartCode { get; set; }
        public string CateSym { get; set; }
        public string Category { get; set; }
        public string CategoryLangZH { get; set; }
        public string CategoryLangVN { get; set; }
        public string CategoryLangEN { get; set; }
        public string DateStart { get; set; }
        public string DateStartExcel { get; set; }
        public DateTime? DateStartOrder { get; set; }
        public string DateEnd { get; set; }
        public string DateEndExcel { get; set; }
        public string LeaveDayByString { get; set; }
        public string Status { get; set; }
        public string StatusExcel { get; set; }
        public string TimeStart { get; set; }
        public string TimeEnd { get; set; }
        public string CreatedString { get; set; }
        public string LeaveDayString { get; set; }
        public string SearchDate { get; set; }
        public string UpdatedString { get; set; }
        public string LeaveDayByRangerDateString { get; set; }
        public double? LeaveDayByRangerDate { get; set; }
        public string Sender { get; set; }
    }
}