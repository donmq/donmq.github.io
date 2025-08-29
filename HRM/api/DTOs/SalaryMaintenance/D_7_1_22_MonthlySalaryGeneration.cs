namespace API.DTOs.SalaryMaintenance
{
    public class D_7_22_MonthlySalaryGeneration
    {

    }

    public class MonthlySalaryGenerationParam
    {
        public string Factory { get; set; }
        public string Year_Month { get; set; }
        public List<string> Permission_Group { get; set; }
        public string Employee_ID { get; set; }
        public bool Is_Delete { get; set; }
        public string UserName { get; set; }
    }

    public class MonthlyDataLockParam
    {
        public string Factory { get; set; }
        public string Year_Month { get; set; }
        public List<string> Permission_Group { get; set; }
        public string Salary_Lock { get; set; }
        public string UserName { get; set; }
    }
}