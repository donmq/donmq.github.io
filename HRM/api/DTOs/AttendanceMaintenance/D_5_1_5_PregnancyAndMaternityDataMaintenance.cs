namespace API.DTOs.AttendanceMaintenance
{
    public class EmpInfo
    {
        public string USER_GUID { get; set; }
        public string Local_Full_Name { get; set; }
        public string Department_Code { get; set; }
        public string Factory { get; set; }
        public string Work_Shift_Type { get; set; }
        public string Work8hours { get; set; }
        public string Work_Type { get; set; }
        public string Special_Regular_Work_Type { get; set; }
        public string Employment_Status { get; set; }
    }

    public class PregnancyMaternityDetail
    {
        public int Seq { get; set; }
        public string Factory { get; set; }
        public string Employee_ID { get; set; }
        public string USER_GUID { get; set; }
        public string Local_Full_Name { get; set; }
        public string Department_Code { get; set; }
        public string Department_Name { get; set; }
        public string Department_Code_Name { get; set; }
        public string Work_Shift_Type { get; set; }
        public string Work_Shift_Type_Name { get; set; }
        public DateTime Due_Date { get; set; }
        public string Due_Date_Str { get; set; }
        public bool? Work8hours { get; set; }
        public string Work8hours_Str { get; set; }
        public DateTime? Work7hours { get; set; }
        public string Work7hours_Str { get; set; }
        public DateTime? Pregnancy36Weeks { get; set; }
        public string Pregnancy36Weeks_Str { get; set; }
        public DateTime? Maternity_Start { get; set; }
        public string Maternity_Start_Str { get; set; }
        public DateTime? Maternity_End { get; set; }
        public string Maternity_End_Str { get; set; }
        public DateTime? GoWork_Date { get; set; }
        public string GoWork_Date_Str { get; set; }
        public bool? Close_Case { get; set; }
        public string Close_Case_Str { get; set; }
        public string Work_Type_Before { get; set; }
        public string Work_Type_After { get; set; }
        public string Pregnancy_Week { get; set; }
        public string Special_Regular_Work_Type { get; set; }
        public string Remark { get; set; }
        public DateTime? Ultrasound_Date { get; set; }
        public string Ultrasound_Date_Str { get; set; }
        public DateTime? Baby_Start { get; set; }
        public string Baby_Start_Str { get; set; }
        public DateTime? Baby_End { get; set; }
        public string Baby_End_Str { get; set; }
        public DateTime? Estimated_Date1 { get; set; }
        public string Estimated_Date1_Str { get; set; }
        public DateTime? Estimated_Date2 { get; set; }
        public string Estimated_Date2_Str { get; set; }
        public DateTime? Estimated_Date3 { get; set; }
        public string Estimated_Date3_Str { get; set; }
        public DateTime? Estimated_Date4 { get; set; }
        public string Estimated_Date4_Str { get; set; }
        public DateTime? Estimated_Date5 { get; set; }
        public string Estimated_Date5_Str { get; set; }
        public DateTime? Insurance_Date1 { get; set; }
        public string Insurance_Date1_Str { get; set; }
        public DateTime? Insurance_Date2 { get; set; }
        public string Insurance_Date2_Str { get; set; }
        public DateTime? Insurance_Date3 { get; set; }
        public string Insurance_Date3_Str { get; set; }
        public DateTime? Insurance_Date4 { get; set; }
        public string Insurance_Date4_Str { get; set; }
        public DateTime? Insurance_Date5 { get; set; }
        public string Insurance_Date5_Str { get; set; }
        public DateTime? Leave_Date1 { get; set; }
        public string Leave_Date1_Str { get; set; }
        public DateTime? Leave_Date2 { get; set; }
        public string Leave_Date2_Str { get; set; }
        public DateTime? Leave_Date3 { get; set; }
        public string Leave_Date3_Str { get; set; }
        public DateTime? Leave_Date4 { get; set; }
        public string Leave_Date4_Str { get; set; }
        public DateTime? Leave_Date5 { get; set; }
        public string Leave_Date5_Str { get; set; }
        public string Update_By { get; set; }
        public DateTime Update_Time { get; set; }
        public string Update_Time_Str { get; set; }

    }

    public class PregnancyMaternityParam
    {
        public string Factory { get; set; }
        public string Department_Code { get; set; }
        public string Employee_ID { get; set; }
        public string DueDate_Start { get; set; }
        public string DueDate_Start_Str { get; set; }
        public string DueDate_End { get; set; }
        public string DueDate_End_Str { get; set; }
        public string MaternityLeave_Start { get; set; }
        public string MaternityLeave_Start_Str { get; set; }
        public string MaternityLeave_End { get; set; }
        public string MaternityLeave_End_Str { get; set; }
        public string Language { get; set; }
    }
    public class PregnancyMaternity_TypeheadKeyValue
    {
        public string Key { get; set; }
        public string USER_GUID { get; set; }
    }
}