
namespace API.DTOs.SalaryReport
{
    public class TaxPayingEmployeeMonthlyNightShiftExtraPayAndOvertimePay
    {

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