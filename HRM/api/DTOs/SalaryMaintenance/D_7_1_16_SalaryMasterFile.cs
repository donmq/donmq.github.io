using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs.SalaryMaintenance
{
    public class SalaryMasterFile_Main
    {
        public string USER_GUID { get; set; }
        public string Division { get; set; }
        public string Factory { get; set; }
        public string Employee_ID { get; set; }
        public string Local_Full_Name { get; set; }
        public string Employment_Status { get; set; }
        public string Department { get; set; }
        public string Department_Str { get; set; }
        public decimal Position_Grade { get; set; }
        public string Position_Title { get; set; }
        public string Position_Title_Str { get; set; }
        public string Permission_Group { get; set; }
        public string Permission_Group_Str { get; set; }
        public string ActingPosition_Start { get; set; }
        public string ActingPosition_End { get; set; }
        public string Technical_Type { get; set; }
        public string Technical_Type_Str { get; set; }
        public string Expertise_Category { get; set; }
        public string Expertise_Category_Str { get; set; }
        public string Salary_Type { get; set; }
        public string Salary_Type_Str { get; set; }
        public decimal Salary_Grade { get; set; }
        public decimal Salary_Level { get; set; }
        public string Currency { get; set; }
        public string Update_By { get; set; }
        public string Update_Time { get; set; }
        public string Effective_Date { get; set; }
        public string Onboard_Date { get; set; }
    }

    public class SalaryMasterFile_Param
    { 
        public string Factory { get; set; }
        public string Department { get; set; }
        public string Employee_ID { get; set; }
        public string Employment_Status { get; set; }
        public string Position_Title { get; set; }
        public List<string> Permission_Group { get; set; }
        public string Salary_Type { get; set; }
        public string Salary_Grade { get; set; }
        public string Salary_Level { get; set; }
        public string Language { get; set; }
    }
    public class SalaryMasterFile_Detail
    { 
        public PaginationUtility<SalaryItem> SalaryItemsPagination { get; set; }
        public int Total_Salary { get; set; }
    }

    public class SalaryItem
    { 
        public string Salary_Item { get; set; }
        public string Employee_ID { get; set; }
        public string Salary_Item_Name { get; set; }
        public string Salary_Item_NameTW { get; set; }
        public int Amount { get; set; }
    }

}