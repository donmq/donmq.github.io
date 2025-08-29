namespace API.DTOs.SalaryMaintenance
{
    public class D_7_13_IncomeTaxBracketSettingDto
    {

    }

    public class IncomeTaxBracketSettingMain
    {
        public string Nation { get; set; }
        public DateTime Effective_Month { get; set; }
        public string Effective_Month_Str { get; set; }
        public string Tax_Code { get; set; }
        public string Tax_Code_Name { get; set; }
        public string Type { get; set; }
        public int Tax_Level { get; set; }
        public decimal Income_Start { get; set; }
        public decimal Income_End { get; set; }
        public decimal Rate { get; set; }
        public decimal Deduction { get; set; }
        public string Update_By { get; set; }
        public DateTime Update_Time { get; set; }
        public bool Is_Disabled { get; set; }
    }

    public class IncomeTaxBracketSettingDto : IncomeTaxBracketSettingMain
    {
        public List<IncomeTaxBracketSetting_SubData> SubData { get; set; }
    }
    public class IncomeTaxBracketSetting_SubData
    {
        public int Tax_Level { get; set; }
        public decimal Income_Start { get; set; }
        public decimal Income_End { get; set; }
        public decimal Rate { get; set; }
        public decimal Deduction { get; set; }
        public string Update_By { get; set; }
        public DateTime Update_Time { get; set; }
        public string Update_Time_Str { get; set; }
    }

    public class IncomeTaxBracketSettingParam
    {
        public string Nationality { get; set; }
        public string Tax_Code { get; set; }
        public string Start_Effective_Month { get; set; }
        public string End_Effective_Month { get; set; }
        public string Language { get; set; }
    }

    public class IncomeTaxBracketSettingSub
    {
        public string Tax_Code { get; set; }
        public string Type { get; set; }
    }
}