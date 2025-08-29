using API.Models;

namespace API.DTOs.SalaryMaintenance
{
    public class BankAccountMaintenanceDto
    {
        public string USER_GUID { get; set; }
        public string Factory { get; set; }
        public string Employee_ID { get; set; }
        public string Local_Full_Name { get; set; }
        public string BankNo { get; set; }
        public string Bank_Code {get; set;}
        public string Create_Date { get; set; }
        public string Update_By { get; set; }
        public string Update_Time { get; set; }
    }

    public class BankAccountMaintenanceParam
    {
        public string Factory { get; set; }
        public string Employee_ID { get; set; }
        public string Language { get; set; }
    }
    public class BankAccountMaintenance_Personal
    {
        public string USER_GUID { get; set; }
        public string Local_Full_Name { get; set; }
    }

    public class BankAccountMaintenanceUpload
    {
        public IFormFile File { get; set; }
        public string Language { get; set; }
    }

    public class BankAccountMaintenanceReport : HRMS_Sal_Bank_Account
    {
        public string Error_Message { get; set; }
        public string Create_Date_Str { get; set; }
        public string IsCorrect { get; set; }
    }
}