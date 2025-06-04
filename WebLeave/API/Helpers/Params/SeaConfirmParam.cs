namespace API.Helpers.Params
{
    public class SeaConfirmParam
    {
        public string EmpNumber { get; set; }
        public double? LeaveDay { get; set; }
        public int? DeptID { get; set; }
        public int? CateID { get; set; }
        public int? PartID { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
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
        public string Status1 { get; set; }
        public string Status2 { get; set; }
        public string Status3 { get; set; }
        public string Status4 { get; set; }
    }
}