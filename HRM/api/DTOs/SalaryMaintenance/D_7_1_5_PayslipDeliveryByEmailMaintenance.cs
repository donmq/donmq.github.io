namespace API.DTOs.SalaryMaintenance
{
    public class D_7_5_PayslipDeliveryByEmailMaintenanceDto
    {
        public string USER_GUID { get; set; }
        public string Factory { get; set; }
        public string Employee_ID { get; set; }
        public string Local_Full_Name { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }
        public string Update_By { get; set; }
        public string Update_Time { get; set; }
        public bool isDeleteDisable { get; set; }
    }

    public class D_7_5_PayslipDeliveryByEmailMaintenanceReport
    {
        public string Factory { get; set; }
        public string Employee_ID { get; set; }
        public string Local_Full_Name { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }
        public string Update_By { get; set; }
        public string Update_Time { get; set; }
        public string IsCorrect { get; set; }
        public string Error_Message { get; set; }
    }

    public class EmployeeData
    {
        public string USER_GUID { get; set; }
        public string Local_Full_Name { get; set; }
    }
}