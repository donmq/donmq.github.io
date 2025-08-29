namespace API.DTOs.SalaryReport
{
    public class D_7_2_7_MonthlySalaryAdditionsDeductionsSummaryReport
    {

    }

    public class MonthlySalaryAdditionsDeductionsSummaryReportParam
    {
        public string Factory { get; set; }
        public string Year_Month { get; set; }
        public string Kind { get; set; }
        public List<string> Permission_Group { get; set; }
        public string Department { get; set; }
        public string UserName { get; set; }
        public string Language { get; set; }

    }

    public class MonthlySalaryAdditionsDeductionsSummaryReportData
    {
        public string Permission_Group { get; set; }
        public string Permission_Group_Title { get; set; }

        public string AddDed_Type { get; set; }
        public string AddDed_Type_Title { get; set; }

        public string AddDed_Item { get; set; }
        public string AddDed_Item_Title { get; set; }

        public decimal Amount { get; set; }
    }
}