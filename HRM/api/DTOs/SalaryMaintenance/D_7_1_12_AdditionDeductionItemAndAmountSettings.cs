namespace API.DTOs.SalaryMaintenance
{
    public class AdditionDeductionItemAndAmountSettingsDto
    {
        public string Factory { get; set; }
        public string Permission_Group { get; set; }
        public string Permission_Group_Title { get; set; }
        public string Salary_Type { get; set; }
        public string Salary_Type_Title { get; set; }
        public DateTime Effective_Month { get; set; }
        public string Effective_Month_Str { get; set; }
        public string AddDed_Type { get; set; }
        public string AddDed_Type_Title { get; set; }
        public string AddDed_Item { get; set; }
        public string AddDed_Item_Title { get; set; }
        public int Amount { get; set; }
        public string Onjob_Print { get; set; }
        public string Resigned_Print { get; set; }
        public string Update_By { get; set; }
        public DateTime Update_Time { get; set; }
        public bool IsDisable { get; set; }

    }

    public class AdditionDeductionItemAndAmountSettingsParam
    {
        public string Factory { get; set; }
        public List<string> Permission_Group { get; set; } = new();
        public string Salary_Type { get; set; }
        public string Effective_Month { get; set; }
        public string AddDed_Type { get; set; }
        public string AddDed_Item { get; set; }
        public string Language { get; set; }
    }


    public class AdditionDeductionItemAndAmountSettings_SubParam
    {
        public string Factory { get; set; }
        public string Permission_Group { get; set; }
        public string Salary_Type { get; set; }
        public DateTime Effective_Month { get; set; }
        public string Effective_Month_Str { get; set; }
        public bool Is_Duplicate { get; set; }
    }

    public class AdditionDeductionItemAndAmountSettings_SubData
    {
        public string AddDed_Type { get; set; }
        public string AddDed_Type_Title { get; set; }
        public string AddDed_Item { get; set; }
        public string AddDed_Item_Title { get; set; }
        public int Amount { get; set; }
        public string Onjob_Print { get; set; }
        public string Resigned_Print { get; set; }
        public string Update_By { get; set; }
        public DateTime Update_Time { get; set; }
        public string Update_Time_Str { get; set; }
        public bool Is_Duplicate { get; set; }
    }

    public class AdditionDeductionItemAndAmountSettings_Form
    {
        public List<AdditionDeductionItemAndAmountSettings_SubData> Data { get; set; }
        public AdditionDeductionItemAndAmountSettings_SubParam Param { get; set; }
    }
}