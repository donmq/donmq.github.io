namespace API.DTOs.SalaryMaintenance
{
    public class UtilityWorkersQualificationSeniorityPrintingDto
    {
        public UtilityWorkersQualificationSeniorityPrintingDto()
        {
            this.Update_Amount = null;
            this.Update_Technical_Allowance = null;
        }
        public string Factory { get; set; }
        public string Department { get; set; }
        public string Department_Name { get; set; }
        public string Employee_ID { get; set; }
        public string Local_Full_Name { get; set; }
        public DateTime? Onboard_Date { get; set; }
        public decimal Year { get; set; }
        public string Work_Type { get; set; }
        public string Work_Type_Title { get; set; }
        public int Number_Of_Months_After_Utility_Qualification { get; set; }
        public DateTime? Wpassdate { get; set; }
        public DateTime? Update_Amount { get; set; }
        public int Technical_Allowance { get; set; }
        public DateTime? Update_Technical_Allowance { get; set; }
        public string Update_By { get; set; }
        public DateTime? Update_Time { get; set; }
    }
    public class UtilityWorkersQualificationSeniorityPrintingParam
    {
        public string Factory { get; set; }
        public string YearMonth { get; set; }
        public int NumberOfMonth { get; set; }
        public string Department { get; set; }
        public string Employee_ID { get; set; }
        public string Language { get; set; }
    }
}