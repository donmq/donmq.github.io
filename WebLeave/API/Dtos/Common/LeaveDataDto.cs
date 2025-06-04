namespace API.Dtos.Common
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
        public string ReasonAdjust { get; set; }

        public string EmpName { get; set; }
        public string EmpNumber { get; set; }
        public string PartCode { get; set; }
        public string PartCodeTruncate { get; set; }
        public string CateSym { get; set; }
        public string CateNameVN { get; set; }
        public string CateNameEN { get; set; }
        public string CateNameZH { get; set; }
        public string DeptNameVN { get; set; }
        public string DeptNameEN { get; set; }
        public string DeptNameZH { get; set; }
        public string CateName_vi { get; set; }
        public string CateName_en { get; set; }
        public string CateName_zh { get; set; }
        public string DeptName_vi { get; set; }
        public string DeptName_en { get; set; }
        public string DeptName_zh { get; set; }
        public string LeaveDayByString { get; set; }
        public bool? exhibit { get; set; }
        public int PartID { get; set; }
        public string PartNameVN { get; set; }
        public string PartNameEN { get; set; }
        public string PartNameZH { get; set; }
        public string PartName_vi { get; set; }
        public string PartName_en { get; set; }
        public string PartName_zh { get; set; }
        public DateTime? DateIn { get; set; }
        public int? CommentLeave { get; set; }
        public string DeptCode { get; set; }
        public string ApprovedString { get; set; }
        public int DeptID { get; set; }
        public string Sender { get; set; }
        public string SendBy { get; set; }


        // Sử dụng cho Main Detail
        public string NumberDay { get; set; } // Số Ngày
        public string Category { get; set; } // Loại phép
        public string StatusString { get; set; } // Tình Trạng

        // Sử dụng cho Main Sub Detail
        public string FullName { get; set; }

        public int? GBID { get; set; }
        public string PartSym { get; set; }
        public string DeptSym { get; set; }
        public string BuildingSym { get; set; }
        public string AreaSym { get; set; }

        public string ApproveName { get; set; } // Tên người gửi
        public int RoleEditComments { get; set; } // Quyền được chỉnh sửa ghi chú SEA/HR xác nhận
        public List<string> CommentList { get; set; } // Danh sách comments
        public string DepartmentName { get; set; } // Bộ phận [A09 - A01-Bảo vệ]
        public List<string> ApproveList { get; set; } // Danh sách người duyệt
        public string LunchBreakVN { get; set; }
        public string LunchBreakEN { get; set; }
        public string LunchBreakZH { get; set; }

    }

    public class LeaveDetailDto
    {
        public bool EditCommentArchive { get; set; }
        public bool NotiUser { get; set; }
        public bool RoleApproved { get; set; }
        public bool EnablePreviousMonthEditRequest { get; set; }
        public LeaveDataDto LeaveData { get; set; }
        public List<string> ApprovalPersons { get; set; }
        public List<LeaveDeleteDetail> DeletedLeaves { get; set; }
    }

    public class ExportExcelSeaHr
    {
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
        public string Status { get; set; }
        public string Update { get; set; }
        public string SEAHRNote { get; set; }
    }

    public class LeaveDeleteDetail
    {
        public string TimeLine { get; set; }
        public string Comment { get; set; }
        public string CateNameVN { get; set; }
        public string CateNameEN { get; set; }
        public string CateNameZH { get; set; }
    }
}