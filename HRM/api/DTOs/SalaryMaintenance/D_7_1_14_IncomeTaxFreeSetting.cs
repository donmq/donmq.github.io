namespace API.DTOs.SalaryMaintenance
{
    public class IncomeTaxFreeSetting_MainData
    {
        public string Factory { get; set; }
        public string Type { get; set; }
        public string Type_Name { get; set; }
        public string Salary_Type { get; set; }
        public string Salary_Type_Name { get; set; }
        public string Effective_Month { get; set; }
        public decimal Amount { get; set; }
        public string Update_By { get; set; }
        public DateTime Update_Time { get; set; }
        public bool Is_Disabled { get; set; }
    }

    public class IncomeTaxFreeSetting_MainParam
    {
        public string Factory { get; set; }
        public string Type { get; set; }
        public string Salary_Type { get; set; }
        public string Start_Effective_Month { get; set; }
        public string End_Effective_Month { get; set; }
        public string Language { get; set; }
    }

    public class IncomeTaxFreeSettingDto
    {
        public string Factory { get; set; }
        public string Type { get; set; }
        public string Salary_Type { get; set; }
        public DateTime Effective_Month { get; set; }
        public string Effective_Month_Str { get; set; }
        public decimal Amount { get; set; }
        public string Update_By { get; set; }
        public DateTime Update_Time { get; set; }
        public string Update_Time_Str { get; set; }
    }

    public class IncomeTaxFreeSetting_Form
    {
        public List<IncomeTaxFreeSetting_SubData> Data { get; set; }
        public IncomeTaxFreeSetting_SubParam Param { get; set; }
    }

    public class IncomeTaxFreeSetting_SubData
    {
        public string Type { get; set; }
        public string Amount { get; set; }
        public string Update_By { get; set; }
        public DateTime Update_Time { get; set; }
        public string Update_Time_Str { get; set; }
        public bool Is_Duplicate { get; set; }
        public bool Is_Disabled_Edit { get; set; }
    }

    public class IncomeTaxFreeSetting_SubParam
    {
        public string Factory { get; set; }
        public string Salary_Type { get; set; }
        public string Effective_Month { get; set; }
        public string Effective_Month_Str { get; set; }
        public bool Is_Duplicate { get; set; }
    }
}