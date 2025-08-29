using API.Models;

namespace API.DTOs.SalaryMaintenance
{
    public class D_7_2_1_SalaryApprovalForm_Data
    {
        public D_7_2_1_SalaryApprovalForm_Data()
        {
            Effective_Date = null;
        }
        public string Local_Full_Name { get; set; } // template Salary_report =M
        public DateTime? Onboard_Date { get; set; } // template Salary_report =M
        public DateTime? Effective_Date { get; set; }
        public DateTime? WPassDate1 { get; set; }
        public DateTime? WPassDate2 { get; set; }
        public DateTime? Previous_Adjustment_Date { get; set; } // template Salary_report =M
        public string Department { get; set; }  // template Salary_report =M
        public string Department_Name { get; set; }  // template Salary_report =M
        public string SalaryGrade_SalaryLevel { get; set; }
        public string Salary_Type { get; set; }
        public string Salary_Type_Name { get; set; }
        public string Technical_Type { get; set; }
        public string Technical_Type_Name { get; set; }
        public string Expertise_Category { get; set; }
        public string Expertise_Category_Name { get; set; }
        public string Employee_ID { get; set; } // template Salary_report =M
        public string Position_Title { get; set; }
        public string Position_Title_Name { get; set; }   // template Salary_report =M
        public int? Seniority { get; set; }  // template Salary_report =M
        public List<SalaryDetailValueDto> SalaryDetailValues { get; set; }
    }
    public class D_7_2_1_SalaryApprovalForm_Param
    {
        public string Factory { get; set; }
        public string Kind { get; set; }
        public List<string> Permission_Group { get; set; }
        public string Department { get; set; }
        public string Employee_ID { get; set; }
        public string Position_Title { get; set; }
        public string Language { get; set; }
    }
    public class SalaryDetailValueDto
    {
        public string SalaryItem { get; set; }
        public KeyValuePair<string, string> SalaryItem_Name { get; set; }
        public int Amount { get; set; }
        public int Seq { get; set; }
        public string USER_GUID { get; set; }
        public string Division { get; set; }
        public string Factory { get; set; }
        public string Employee_ID { get; set; }
        public string Update_By { get; set; }
        public DateTime? Update_Time { get; set; }
    }
}