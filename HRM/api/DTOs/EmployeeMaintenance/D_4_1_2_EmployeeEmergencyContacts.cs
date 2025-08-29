
namespace API.DTOs.EmployeeMaintenance
{
    public class EmployeeEmergencyContactsDto
    {
        public string USER_GUID { get; set; }
        public string Division { get; set; }
        public string Factory { get; set; }
        public string Employee_ID { get; set; }
        public string LocalFullName { get; set; }
        public string Nationality { get; set; }
        public string Identification_Number { get; set; }
        public int Seq { get; set; }
        public string Emergency_Contact { get; set; }
        public string Relationship { get; set; }
        public string Emergency_Contact_Phone { get; set; }
        public string Temporary_Address { get; set; }
        public string Emergency_Contact_Address { get; set; }
        public string Update_By { get; set; }
        public DateTime Update_Time { get; set; }
    }

    public class EmployeeEmergencyContactsParam
    {
        public string USER_GUID { get; set; }
        public string Language { get; set; }
    }

    public class DataMain
    {
        public List<EmployeeEmergencyContactsDto> Result { get; set; }
        public int TotalCount { get; set; }
    }

    public class EmployeeEmergencyContactsReport
    {
        public string Division { get; set; }
        public string Factory { get; set; }
        public string Employee_ID { get; set; }
        public string Emergency_Contact { get; set; }
        public string Relationship { get; set; }
        public string Emergency_Contact_Phone { get; set; }
        public string Temporary_Address { get; set; }
        public string Emergency_Contact_Address { get; set; }
        public string Error_Message { get; set; }
    }

    public class EmployeeEmergencyContacts_UploadResult
    {
        public int Total { get; set; }
        public int Success { get; set; }
        public int Error { get; set; }
        public byte[] ErrorReport { get; set; }
    }
}