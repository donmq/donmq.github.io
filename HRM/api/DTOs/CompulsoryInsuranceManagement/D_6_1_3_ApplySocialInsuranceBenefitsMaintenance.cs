
namespace API.DTOs.CompulsoryInsuranceManagement
{
    public class ApplySocialInsuranceBenefitsMaintenanceDto
    {
        public string USER_GUID { get; set; }
        public string Factory { get; set; }
        public string Employee_ID { get; set; }
        public string Local_Full_Name { get; set; }
        public DateTime Declaration_Month { get; set; }
        public string Declaration_Month_Str { get; set; }
        public short Declaration_Seq { get; set; }
        public string Benefits_Kind { get; set; }
        public string Benefits_Name { get; set; }
        public string Special_Work_Type { get; set; }
        public string Work_Type { get; set; }
        public DateTime? Birthday_Child { get; set; }
        public string Birthday_Child_Str { get; set; }
        public DateTime Benefits_Start { get; set; }
        public string Benefits_Start_Str { get; set; }
        public DateTime Benefits_End { get; set; }
        public string Benefits_End_Str { get; set; }
        public string Benefits_Num { get; set; }
        public short Total_Days { get; set; }
        public int Amt { get; set; }
        public string Compulsory_Insurance_Number { get; set; }
        public int Annual_Accumulated_Days { get; set; }
        public string Update_By { get; set; }
        public DateTime Update_Time { get; set; }
        public string Update_Time_Str { get; set; }
        public bool is_Edit { get; set; }
    }

    public class ApplySocialInsuranceBenefitsMaintenanceParam
    {
        public string Factory { get; set; }
        public string Start_Year_Month { get; set; }
        public string Start_Year_Month_Str { get; set; }
        public string End_Year_Month { get; set; }
        public string End_Year_Month_Str { get; set; }

        public short Declaration_Seq { get; set; }
        public string Benefits_Kind { get; set; }
        public string Employee_ID { get; set; }
        public string Language { get; set; }
    }

}