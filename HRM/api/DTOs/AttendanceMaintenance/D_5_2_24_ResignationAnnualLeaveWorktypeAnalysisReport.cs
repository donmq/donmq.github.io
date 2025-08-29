namespace API.DTOs.AttendanceMaintenance
{
    public class ResignationAnnualLeaveWorktypeAnalysisReportParam
    {
        public string Factory { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public List<string> Permission_Group { get; set; }
        public string Level { get; set; }
        public string Lang { get; set; }
    }
    public class ResignationAnnualLeaveWorktypeAnalysisReportExcelResult
    {
        public string Department { get; set; }
        public string DepartmentName { get; set; }
        public int w_outs { get; set; }
        public int w_y12 { get; set; }
        public int w_y23 { get; set; }
        public int w_y34 { get; set; }
        public int w_y45 { get; set; }
        public int w_y56 { get; set; }
        public int w_y6 { get; set; }
        public int w_all { get; set; }
        public int w_mh { get; set; }
        public int w_n1 { get; set; }
        public int w_m1 { get; set; }
        public int w_news { get; set; }
        public int w_y01 { get; set; }
        public List<WorkTypeList> WorkTypeList { get; set; }
        public List<WorkTypeList> WorkTypeListThan1 { get; set; }
        

    }
    public class WorkTypeList
    {
        public string WorkType { get; set; }
        public decimal Year { get; set; }
        public string WorkTypeName { get; set; }
        public int Value { get; set; }    
    }
}