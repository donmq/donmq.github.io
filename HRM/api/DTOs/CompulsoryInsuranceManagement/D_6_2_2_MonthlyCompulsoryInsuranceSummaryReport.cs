
namespace API.DTOs.CompulsoryInsuranceManagement
{
    public class MonthlyCompulsoryInsuranceSummaryReportDto
    {
        public string Factory { get; set; }
        public string Employee_ID { get; set; }
        public string Department { get; set; }
        public string Department_Name { get; set; }
        public string PermissionGroup { get; set; }
        public string Salary_Type { get; set; }
        public decimal Basic_Amt { get; set; } = 0;
        public decimal Employee_Amt { get; set; } = 0;
        public decimal Employer_Amt { get; set; }
    }


    public class MonthlyCompulsoryInsuranceSummaryReportExcel
    {
        public string Department { get; set; }
        public string Department_Name { get; set; }
        public int Number_Of_Employees { get; set; }
        public int Insured_Salary { get; set; }
        public int Employer_Contribution { get; set; }
        public int Employee_Contribution { get; set; }
        public int Total_Amount { get; set; }
    }

    public class Total
    {
        public int Total_Number_Employees { get; set; }
        public int Total_Insured_Salary { get; set; }
        public int ToTal_Employer_Contribution { get; set; }
        public int ToTal_Employee_Contribution { get; set; }
        public int Total_Amount { get; set; }

    }
    public class MonthlyCompulsoryInsuranceSummaryReport_Param
    {
        public string Factory { get; set; }
        public string Year_Month { get; set; }
        public string Department { get; set; }
        public List<string> Permission_Group { get; set; } = new();
        public List<string> Permission_Group_Name { get; set; } = new();
        public string Insurance_Type { get; set; }
        public string Insurance_Type_Full { get; set; }
        public string Kind { get; set; }
        public string Language { get; set; }
    }
}