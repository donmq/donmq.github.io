namespace API.DTOs.SalaryMaintenance
{
    public class D_7_6_PersonalIncomeTaxNumberMaintenanceDto
    {
        public string USER_GUID { get; set; }
        public string Factory { get; set; }
        public string Year { get; set; }
        public string Employee_ID { get; set; }
        public string Department_Code { get; set; }
        public string Department_Name { get; set; }
        public string Department_Code_Name { get; set; }
        public string Local_Full_Name { get; set; }
        public string TaxNo { get; set; }
        public string Dependents { get; set; }
        public string Update_By { get; set; }
        public string Update_Time { get; set; }
    }

    public class D_7_6_PersonalIncomeTaxNumberMaintenanceReport
    {
        public string Factory { get; set; }
        public string Year { get; set; }
        public string Employee_ID { get; set; }
        public string TaxNo { get; set; }
        public string Dependents { get; set; }
        public string Update_By { get; set; }
        public string Update_Time { get; set; }
        public string IsCorrect { get; set; }
        public string Error_Message { get; set; }
    }

    public class EmployeeInfo
    {
        public string USER_GUID { get; set; }
        public string Local_Full_Name { get; set; }
        public string Department { get; set; }
    }
}