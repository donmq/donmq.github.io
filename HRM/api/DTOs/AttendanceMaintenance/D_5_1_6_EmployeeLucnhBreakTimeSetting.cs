namespace API.DTOs.AttendanceMaintenance
{
    public class HRMS_Att_LunchtimeDto
    {
        public string USER_GUID { get; set; }
        public string Factory { get; set; }
        public string Department_Code { get; set; }
        public string Department_Name { get; set; }
        public string Department_Code_Name { get; set; }
        public string Employee_ID { get; set; }
        public string Local_Full_Name { get; set; }
        public string Lunch_Start { get; set; }
        public string Lunch_End { get; set; }
        public string Update_By { get; set; }
        public string Update_Time { get; set; }
    }

    public class HRMS_Att_LunchtimeReport
    {
        public string Factory { get; set; }
        public string Employee_ID { get; set; }
        public string Lunch_Start { get; set; }
        public string Lunch_End { get; set; }
        public string IsCorrect { get; set; }
        public string Error_Message { get; set; }
    }
    public class EmployeeLunchBreakTimeSettingParam
    {
        public string Factory { get; set; }
        public string In_Service { get; set; }
        public string Employee_ID { get; set; }
        public string Department_From { get; set; }
        public string Department_To { get; set; }
        public string Lang { get; set; }
    }
}