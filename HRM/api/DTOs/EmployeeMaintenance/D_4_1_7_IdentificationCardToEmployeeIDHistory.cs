namespace API.DTOs.EmployeeMaintenance
{
    public class IdentificationCardToEmployeeIDHistoryParam
    {
        public string Division { get; set; }
        public string Factory { get; set; }
        public string Employee_ID { get; set; }
        public string Nationality { get; set; }
        public string Identification_Number { get; set; }
        public string Lang { get; set; }
    }
    public partial class HRMS_Emp_IDcard_EmpID_HistoryDto
    {
        public string Nationality { get; set; }
        public string Identification_Number { get; set; }
        public string Local_Full_Name { get; set; }
        public string Division { get; set; }
        public string Factory { get; set; }
        public string Employee_ID { get; set; }
        public string Department { get; set; }
        public string Assigned_Division { get; set; }
        public string Assigned_Factory { get; set; }
        public string Assigned_Employee_ID { get; set; }
        public string Assigned_Department { get; set; }
        public DateTime Onboard_Date { get; set; }
        public DateTime? Resign_Date { get; set; }
        public string Update_By { get; set; }
        public DateTime Update_Time { get; set; }
    }
}