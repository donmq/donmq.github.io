using API.Models;

namespace API.DTOs.SalaryReport
{
    public class MonthlySalarySummaryReportParam
    {
        public string Factory { get; set; }
        public string Year_Month { get; set; }
        public string Year_Month_Str { get; set; }
        public string Kind { get; set; }
        public List<string> Permission_Group { get; set; } = new();
        public string Transfer { get; set; }
        public string Report_Kind { get; set; }
        public string Level { get; set; }
        public string Department { get; set; }
        public string Lang { get; set; }
    }

    public class MonthlySalarySummaryReportDto
    {
        public string Department { get; set; }
        public string Department_Name { get; set; }
        public int Department_Headcount { get; set; }
        public List<KeyValuePair<string, decimal>> Salary_Item { get; set; } = new();
        public decimal Overtime_Allowance { get; set; }
        public decimal Night_Shift_Allowance { get; set; }
        public decimal Other_Additions { get; set; }
        public decimal Total_Addition_Item { get; set; }
        public List<KeyValuePair<string, decimal>> Insurance_Deduction { get; set; }
        public int Tax { get; set; }
        public decimal Other_Deductions { get; set; }
        public decimal Total_Deduction_Item { get; set; }
        public decimal Net_Amount_Received { get; set; }
    }

    public class Sal_Monthly_7_2_4
    {
        public string Employee_ID { get; set; }
        public string Department { get; set; }
        public string Permission_Group { get; set; }
        public string Salary_Type { get; set; }
        public int Tax { get; set; }
    }
    public class EmployeeSalaryInfo
    {
        public string Employee_ID { get; set; }
        public string Department { get; set; }
        public string Department_Name { get; set; }
        public List<KeyValuePair<string, decimal>> SalaryDetails { get; set; } = new();
        public decimal OvertimeAllowance { get; set; }
        public decimal NightShiftAllowance { get; set; }
        public decimal OtherAdditions { get; set; }
        public decimal TotalAddtionsItem { get; set; }
        public List<KeyValuePair<string, decimal>> InsuranceDeductionDetails { get; set; }
        public int Tax { get; set; }
        public decimal OtherDeductions { get; set; }
        public decimal TotalDeductionItem { get; set; }
        public decimal NetAmountReceived { get; set; }
    }

    public class SalaryDetailResult
    {
        public string Employee_ID { get; set; }
        public string Item { get; set; }
        public decimal Amount { get; set; }
        public string TypeSeq { get; set; }
        public string AddedType { get; set; }
    }

    #region Query_Sal_Monthly_Detail
    public class Sal_Monthly_Detail_Temp_7_2_4
    {
        public string Employee_ID { get; set; }
        public string Item { get; set; }
        public int Amount { get; set; }
    }

    public class Sal_Setting_Temp_7_2_4
    {
        public int Seq { get; set; }
        public string Salary_Item { get; set; }
        public string Permission_Group { get; set; }
        public string Salary_Type { get; set; }
    }

    public class Sal_Monthly_Detail_Values_7_2_4
    {
        public int Seq { get; set; }
        public string Employee_ID { get; set; }
        public string Permission_Group { get; set; }
        public string Salary_Type { get; set; }
        public string Item { get; set; }
        public int Amount { get; set; }
        public string Code { get; set; }
    }
    #endregion
}