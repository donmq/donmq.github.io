using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class D_6_2_1_MonthlyCompulsoryInsuranceDetailedReportSource
{
    public string USER_GUID { get; set; }
    public string Local_Full_Name { get; set; }
    public string Factory { get; set; }
    public DateTime Sal_Month { get; set; }
    public string Employee_ID { get; set; }
    public string Department { get; set; }
    public string Department_Name { get; set; }
    public string Currency { get; set; }
    public string Permission_Group { get; set; }
    public string Salary_Type { get; set; }
    public decimal Basic_Amt { get; set; } = 0;
    public decimal Employee_Amt { get; set; } = 0;
    public decimal Employer_Amt { get; set; }
    public decimal Total_Amt
    {
        get
        {
            return Employee_Amt + Employer_Amt;
        }
    }
}
public class TotalResult
{
    public decimal Basic_Amt { get; set; }
    public decimal Employee_Amt { get; set; }
    public decimal Employer_Amt { get; set; }
    public decimal Total_Amt { get; set; }
}

public class D_6_2_1_MonthlyCompulsoryInsuranceDetailedReport
{
    public string Department { get; set; }
    public string Department_Name { get; set; }
    public string Employee_ID { get; set; }
    public string Local_Full_Name { get; set; }
    public string Insured_Salary { get; set; }
    public string Employer_Contribution { get; set; }
    public string Employee_Contribution { get; set; }
    public int Total_Amount { get; set; }

}

public class D_6_2_1_MonthlyCompulsoryInsuranceDetailedReportParam
{
    public string Factory { get; set; }
    public string Year_Month { get; set; }
    public List<string> Permission_Group { get; set; }
    public List<string> Permission_Group_Name { get; set; }
    public string Department { get; set; }
    public string Insurance_Type { get; set; }
    public string Insurance_Type_Full { get; set; }
    public string Kind { get; set; }
    public string Language { get; set; }

}

public class D_6_2_1_Department_Report
{
    public string Factory { get; set; }
    public string Department_Code { get; set; }
    public string Department_Name { get; set; }
}