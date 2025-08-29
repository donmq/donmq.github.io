namespace API.DTOs.EmployeeMaintenance
{
    public class ResignationManagementParam
    {
        public string Division { get; set; }
        public string Factory { get; set; }
        public string Employee_ID { get; set; }
        public string Local_Full_Name { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Lang { get; set; }
        public List<string> Role_List { get; set; }
    }

    public class ResignAddAndEditParam : ResignationManagementParam
    {
        public string USER_GUID { get; set; }
        public string Nationality { get; set; }
        public string Identification_Number { get; set; }
        public string Resign_Date { get; set; }
        public string Onboard_Date { get; set; }
        public string Resignation_Type { get; set; }
        public string Resign_Reason { get; set; }
        public string Remark { get; set; }
        public bool? Blacklist { get; set; }
        public string Verifier { get; set; }
        public string Verifier_Name { get; set; }
        public string Verifier_Title { get; set; }
        public string Update_By { get; set; }
        public string Update_Time { get; set; }
    }
    public class HRMS_Emp_ResignationDto
    {
        public string USER_GUID { get; set; }
        public string Division { get; set; }
        public string Factory { get; set; }
        public string Employee_ID { get; set; }
        public string Nationality { get; set; }
        public string Local_Full_Name { get; set; }
        public string Department { get; set; }
        public string Department_Code { get; set; }
        public string Department_Name { get; set; }
        public string Identification_Number { get; set; }
        public string Onboard_Date { get; set; }
        public string Resign_Date { get; set; }
        public string Resignation_Type { get; set; }
        public string Resignation_Type_Str { get; set; }
        public string Resign_Reason { get; set; }
        public string Resign_Reason_Str { get; set; }
        public string Remark { get; set; }
        public bool? Blacklist { get; set; }
        public string Blacklist_Str { get; set; }
        public string Verifier { get; set; }
        public string Verifier_Name { get; set; }
        public string Verifier_Title { get; set; }
        public string Update_By { get; set; }
        public string Update_Time { get; set; }
    }

    public class HRMS_Emp_ResignationFormDto
    {
        public string Nationality { get; set; }
        public string Local_Full_Name { get; set; }
        public string Identification_Number { get; set; }
        public string Onboard_Date { get; set; }
    }
}
