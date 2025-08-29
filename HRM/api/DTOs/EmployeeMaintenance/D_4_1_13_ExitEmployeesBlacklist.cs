
namespace API.DTOs.EmployeeMaintenance
{
    public class HRMS_Emp_BlacklistDto
    {
        public string USER_GUID { get; set; }
        public DateTime Maintenance_Date { get; set; }
        public string Nationality { get; set; }
        public string Identification_Number { get; set; }
        public string Local_Full_Name { get; set; }
        public string Resign_Reason { get; set; }
        public string Description { get; set; }
        public string Update_By { get; set; }
        public DateTime Update_Time { get; set; }
    }
    public class HRMS_Emp_BlacklistParam
    {
        public string Nationality { get; set; }
        public string Identification_Number { get; set; }

    }
}