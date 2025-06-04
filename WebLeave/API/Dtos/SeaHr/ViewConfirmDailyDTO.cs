namespace API.Dtos.SeaHr
{
    public class ViewConfirmDailyDTO
    {
        public int? DeptID { get; set; }
        public string DeptCode { get; set; }
        public string DeptName { get; set; }
        public string DeptNameVN { get; set; }
        public string DeptNameEN { get; set; }
        public string DeptNameZH { get; set; }
        public int? PartID { get; set; }
        public string PartNameVN { get; set; }
        public string PartNameEN { get; set; }
        public string PartNameZH { get; set; }
        public string EmpName { get; set; }
        public string EmpNumber { get; set; }
        public int? CateID { get; set; }
        public string CateSym { get; set; }
        public string CateNameVN { get; set; }
        public string CateNameEN { get; set; }
        public string CateNameZH { get; set; }
        public string TimeStart { get; set; }
        public string TimeEnd { get; set; }
        public string DateStart { get; set; }
        public string DateEnd { get; set; }
        public string LeaveDay { get; set; }
        public string Status { get; set; }
        public string Update { get; set; }
        public string LeaveArchive { get; set; }
        public DateTime? Time_Start { get; set; }
        public DateTime? Time_End { get; set; }
        public int EmpID { get; set; }
        public int? Approved { get; set; }
        public int LeaveID { get; set; }
        public DateTime? UpdateTime { get; set; }
    }

    public class ViewComfirmExport
    {
        public string DeptCode { get; set; }
        public string DeptName { get; set; }
        public string EmpName { get; set; }
        public string EmpNumber { get; set; }
        public string CategoryExcel { get; set; }
        public string TimeStart { get; set; }
        public string TimeEnd { get; set; }
        public string DateStart { get; set; }
        public string DateEnd { get; set; }
        public string LeaveDay { get; set; }
        public string Status { get; set; }
        public string Update { get; set; }
    }

    public class ViewConfirmDailyParam
    {
        public string Lang { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }

        public string Label_PartCode { get; set; }
        public string Label_DeptName { get; set; }
        public string Label_Employee { get; set; }
        public string Label_NumberID { get; set; }
        public string Label_Category { get; set; }
        public string Label_TimeStart { get; set; }
        public string Label_DateStart { get; set; }
        public string Label_TimeEnd { get; set; }
        public string Label_DateEnd { get; set; }
        public string Label_LeaveDay { get; set; }
        public string Label_Status { get; set; }
        public string Label_UpdateTime { get; set; }

    }


}