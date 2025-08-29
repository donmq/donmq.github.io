
using API.Models;

namespace API.DTOs.SalaryReport
{
    public class NightShiftExtraAndOvertimePayReport
    {
        public string Factory { get; set; }
        public string Department { get; set; }
        public string DepartmentName { get; set; }
        public string EmployeeID { get; set; }
        public string LocalFullName { get; set; }
        public string TaxNo { get; set; }
        public string Standard { get; set; }
        public List<decimal> OvertimeHours { get; set; }
        public List<string> OvertimeAndNightShiftAllowance { get; set; }
        public string A06_AMT { get; set; }
        public string Overtime50_AMT { get; set; }
        public string NHNO_AMT { get; set; }
        public string HO_AMT { get; set; }
        public string INS_AMT { get; set; }
        public string SUM_AMT { get; set; }

    }
    public class NightShiftExtraAndOvertimePayParam
    {
        public string Factory { get; set; }
        public string Year_Month { get; set; }
        public List<string> Permission_Group { get; set; }
        public string Department { get; set; }
        public string EmployeeID { get; set; }
        public string UserName { get; set; }
        public string Language { get; set; }

    }
}