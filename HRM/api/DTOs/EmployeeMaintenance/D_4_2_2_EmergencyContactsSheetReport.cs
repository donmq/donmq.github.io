
namespace API.DTOs.EmployeeMaintenance
{
    public class EmergencyContactsReportExport
    {
        public string Division { get; set; }
        public string Factory { get; set; }
        public string EmployeeID { get; set; }
        public string LocalFullName { get; set; }
        public int Seq { get; set; }
        public string EmergencyContact { get; set; }
        public string Relationship { get; set; }
        public string EmergencyContactPhone { get; set; }
        public string TemporaryAddress { get; set; }
        public string EmergencyContactAddress { get; set; }

    }

    public class EmergencyContactsReportParam
    {
        public string EmploymentStatus { get; set; }
        public string Division { get; set; }
        public string Factory { get; set; }
        public string EmployeeID { get; set; }
        public string Department { get; set; }
        public string AssignedDivision { get; set; }
        public string AssignedFactory { get; set; }
        public string AssignedEmployeeID { get; set; }
        public string AssignedDepartment { get; set; }
        public string Language { get; set; }
    }
}