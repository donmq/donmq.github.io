namespace API.Helper.Params.SalaryMaintenance
{
    public class MonthlySalaryMasterFileBackupQueryParam
    {
        public string Year_Month { get; set; }
        public string Year_Month_Str { get; set; }
        public string Factory { get; set; }
        public string Department { get; set; }
        public string Employee_ID { get; set; }
        public string Employment_Status { get; set; }
        public string Position_Title { get; set; }
        public List<string> Permission_Group { get; set; } = new();
        public string Salary_Type { get; set; }
        public string Salary_Grade { get; set; }
        public string Salary_Level { get; set; }
        public string Lang { get; set; }
    }
}