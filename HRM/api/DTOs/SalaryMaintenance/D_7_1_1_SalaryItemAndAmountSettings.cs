
namespace API.DTOs.SalaryMaintenance
{
    public class SalaryItemAndAmountSettings_MainParam
    {
        public string Factory { get; set; }
        public string Salary_Type { get; set; }
        public string Effective_Month { get; set; }
        public string Effective_Month_Str { get; set; }
        public List<string> Permission_Group { get; set; }
        public string FormType { get; set; }
        public string Lang { get; set; }
    }
    public class SalaryItemAndAmountSettings_MainData
    {
        public string Factory { get; set; }
        public string Effective_Month { get; set; }
        public string Permission_Group { get; set; }
        public string Permission_Group_Name { get; set; }

        public string Salary_Type { get; set; }
        public string Salary_Type_Name { get; set; }
        public string Salary_Days { get; set; }
        public string Seq { get; set; }
        public string Salary_Item { get; set; }
        public string Salary_Item_Name { get; set; }
        public string Kind { get; set; }
        public string Kind_Name { get; set; }
        public string Insurance { get; set; }
        public string Amount { get; set; }
        public string Update_By { get; set; }
        public string Update_Time { get; set; }
        public bool Is_Editable { get; set; }
    }
    public class SalaryItemAndAmountSettings_SubParam
    {
        public string Factory { get; set; }
        public string Effective_Month { get; set; }
        public string Effective_Month_Str { get; set; }
        public string Permission_Group { get; set; }
        public string Salary_Type { get; set; }
        public string Salary_Days { get; set; }
    }
    public class SalaryItemAndAmountSettings_SubData
    {
        public string Seq { get; set; }
        public string Salary_Item { get; set; }
        public string Kind { get; set; }
        public string Insurance { get; set; }
        public string Amount { get; set; }
        public string Update_By { get; set; }
        public DateTime Update_Time { get; set; }
        public string Update_Time_Str { get; set; }
        public bool Is_Duplicate { get; set; }
    }
    public class SalaryItemAndAmountSettings_Update
    {
        public SalaryItemAndAmountSettings_SubParam Param { get; set; }
        public List<SalaryItemAndAmountSettings_SubData> Data { get; set; }
    }


}