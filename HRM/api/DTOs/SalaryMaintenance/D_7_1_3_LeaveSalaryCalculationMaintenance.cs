namespace API.DTOs.SalaryMaintenance
{
    public class LeaveSalaryCalculationMaintenanceDTO
    {
        public string Factory { get; set; }
        public string Leave_Code { get; set; }
        public string Leave_Code_Name { get; set; }
        public decimal Salary_Rate { get; set; }
        public string Update_By { get; set; }
        public string Update_Time { get; set; }
    }

    public class LeaveSalaryCalculationMaintenanceParam
    {
        public string Factory { get; set; }
        public string Language { get; set; }
    }
}