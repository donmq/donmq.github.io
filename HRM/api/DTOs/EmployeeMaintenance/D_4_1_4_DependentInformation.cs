namespace API.DTOs.EmployeeMaintenance
{
    public class HRMS_Emp_DependentDto
    {
        public string USER_GUID { get; set; }
        public string Nationality { get; set; }
        public string Identification_Number { get; set; }
        public string Local_Full_Name { get; set; }
        public int Seq { get; set; }
        public string Name { get; set; }
        public string Relationship { get; set; }
        public string Relationship_Name { get; set; }
        public string Occupation { get; set; }
        public bool Dependents { get; set; }
        public string Update_By { get; set; }
        public DateTime Update_Time { get; set; }
    }
    public class HRMS_Emp_DependentParam
    {
        public string Nationality { get; set; }
        public string Identification_Number { get; set; }
        public string USER_GUID { get; set; }
        public string Lang { get; set; }

    }
}