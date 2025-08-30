

namespace API.DTOs.SalaryReport
{
    public class MonthlyUnionDuesSummaryParamReport
    {
        public string Factory { get; set; }
        public string Department { get; set; }
        public string DepartmentName { get; set; }
        public int Union_fee { get; set; }
        public int Medical_Insurance_Fee { get; set; }
        public int TotalAmount { get; set; }
    }
    public class MonthlyUnionDuesSummaryParam
    {
        public string Factory { get; set; }
        public string Year_Month { get; set; }
        public string Department { get; set; }
        public string UserName { get; set; }
        public string Language { get; set; }

    }
}