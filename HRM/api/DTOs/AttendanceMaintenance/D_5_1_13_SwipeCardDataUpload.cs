namespace API.DTOs.AttendanceMaintenance
{
    public class HRMS_Att_Swipe_Card_Upload
    {
        public string Factory { get; set; }

        public IFormFile FileUpload { get; set; }
        public string CurrentUser { get; set; }
    }

    public class HRMS_Att_Swipe_Card_Report
    {
        public string Employee_ID { get; set; }
        public string ErrorMessage { get; set; }

        public HRMS_Att_Swipe_Card_Report() { }
        public HRMS_Att_Swipe_Card_Report(string employeeId, string message) { Employee_ID = employeeId; ErrorMessage = message; }

    }
}