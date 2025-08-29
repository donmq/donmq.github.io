namespace API.DTOs.SalaryReport
{
    public class D_7_2_6_MonthlyNonTransferSalaryPaymentReportParam
    {
        public string Factory { get; set; }
        public string Year_Month { get; set; }
        public List<string> Permission_Group { get; set; }
        public string Department { get; set; }
        public string Employee_ID { get; set; }
        public string Language { get; set; }
    }

    public class D_7_2_6_MonthlyNonTransferSalaryPaymentReportDto
    {
        public string Factory { get; set; }
        public string Year_Month { get; set; }
        public List<string> List_Employee_ID { get; set; }
        public string Department { get; set; }
        public string Employee_ID { get; set; }
        public string Total { get; set; }
        public decimal Actual_Amount { get; set; }
        public decimal tt_h50 { get; set; }
        public decimal tt_200 { get; set; }
        public decimal tt_100 { get; set; }
        public decimal tt_50 { get; set; }
        public decimal tt_20 { get; set; }
        public decimal tt_10 { get; set; }
        public decimal tt_5 { get; set; }
        public decimal tt_2 { get; set; }
        public decimal tt_1 { get; set; }
    }
}