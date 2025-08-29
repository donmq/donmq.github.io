
namespace API.DTOs.EmployeeMaintenance
{
    public class HRMS_Emp_External_ExperienceDto
    {
        public string USER_GUID { get; set; }
        public string Nationality { get; set; }
        public string Identification_Number { get; set; }
        public string Local_Full_Name { get; set; }
        public int Seq { get; set; }
        public string Company_Name { get; set; }
        public string Department { get; set; }
        public bool? Leadership_Role { get; set; }
        public string Position_Title { get; set; }
        public DateTime Tenure_Start { get; set; }
        public DateTime Tenure_End { get; set; }
        public string Update_By { get; set; }
        public DateTime Update_Time { get; set; }
    }
    public class HRMS_Emp_External_ExperienceParam
    {
        public string USER_GUID { get; set; }
    }
}
