
namespace API.DTOs.CompulsoryInsuranceManagement
{
    public class ContributionRateSettingDto
    {
        public string Factory { get; set; }
        public DateTime Effective_Month { get; set; }
        public string Effective_Month_Str { get; set; }
        public string Permission_Group { get; set; }
        public string Permission_Group_Name { get; set; }
        public string Insurance_Type { get; set; }
        public string Insurance_Type_Name { get; set; }
        public string Employer_Rate { get; set; }
        public string Employee_Rate { get; set; }
        public string Update_By { get; set; }
        public string Update_Time { get; set; }
    }

    public class ContributionRateSettingParam
    {
        public string Factory { get; set; }
        public string Effective_Month { get; set; }
        public List<string> Permission_Group { get; set; }
        public string Language { get; set; }
    }

    public class ContributionRateSettingSubParam
    {
        public string Factory { get; set; }
        public DateTime Effective_Month { get; set; }
        public string Effective_Month_Str { get; set; }
        public string Permission_Group { get; set; }
        public string Insurance_Type{ get; set; }
    }

    public class ContributionRateSettingSubData
    {
        public string Insurance_Type { get; set; }
        public string Insurance_Type_Name { get; set; }
        public decimal Employer_Rate { get; set; }
        public decimal Employee_Rate { get; set; }
        public string Update_By { get; set; }
        public DateTime Update_Time { get; set; }
        public string Update_Time_Str { get; set; }
    }

    public class ContributionRateSettingForm
    {
        public ContributionRateSettingSubParam Param { get; set; }
        public ContributionRateSettingSubData[] SubData { get; set; }
    }

    public class ContributionRateSettingCheckEffectiveMonth 
    {
        public bool checkEffective_Month { get; set; }
        public ContributionRateSettingSubData DataDefault { get; set; }
    }

}