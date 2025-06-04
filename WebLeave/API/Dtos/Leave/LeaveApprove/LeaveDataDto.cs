namespace API.Dtos.Leave.LeaveApprove
{
    public class LeaveDataApproveDto
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
        public string CommentLeave { get; set; }
        public bool? LeavePlus { get; set; }
        public bool? LeaveArrange { get; set; }
        public int? UserID { get; set; }
        public int? ApprovedBy { get; set; }
        public int? EditRequest { get; set; }
        public bool? Status_Line { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? CreatedOrderBy { get; set; }
        public DateTime? Updated { get; set; }
        public bool? Status_Lock { get; set; }
        public bool? Lock_Leave { get; set; }
        public string MailContent_Lock { get; set; }
        public string CommentArchive { get; set; }
        //
        public string EmpName { get; set; }
        public int? PartID { get; set; }
        public string CateSym { get; set; }
        public string EmpNumber { get; set; }
        public string DeptIDName { get; set; }
        public string DeptNameLangZH { get; set; }
        public string DeptNameLangEN { get; set; }
        public string DeptNameLangVN { get; set; }
        public string PartNameLang { get; set; }
        public string PartName { get; set; }
        public int? BuildingID { get; set; }
        public int? DeptID { get; set; }
        public int? AreaID { get; set; }
        public int? GBID { get; set; }
        public string BuildingSym { get; set; }
        public string AreaSym { get; set; }
        public string DeptSym { get; set; }
        public string PartSym { get; set; }
        public string DeptCode { get; set; }
        public string PartCode { get; set; }
        public string Category { get; set; }
        public string CategoryLangZH { get; set; }
        public string CategoryLangEN { get; set; }
        public string CategoryLangVN { get; set; }
        public string DateStart { get; set; }
        public DateTime? DateStartOrder { get; set; }
        public string DateEnd { get; set; }
        public string Status { get; set; }
        public string TimeStart { get; set; }
        public string TimeEnd { get; set; }
        public string CreatedString { get; set; }
        public string LeaveDayString { get; set; }
        public string LeaveHourString { get; set; }
        public string SearchDate { get; set; }
        public string PNC { get; set; }
        public string UpdatedString { get; set; }
    }
}