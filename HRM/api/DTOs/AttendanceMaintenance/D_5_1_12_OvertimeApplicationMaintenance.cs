
namespace API.DTOs.AttendanceMaintenance
{
    public class OvertimeApplicationMaintenance_Param
    {
        public string USER_GUID { get; set; }
        public string Factory { get; set; }
        public string Department { get; set; }
        public string Employee_Id { get; set; }
        public string Work_Shift_Type { get; set; }
        public string Overtime_Date { get; set; }
        public string Overtime_Date_Str { get; set; }
        public string Overtime_Date_From { get; set; }
        public string Overtime_Date_From_Str { get; set; }
        public string Overtime_Date_To { get; set; }
        public string Overtime_Date_To_Str { get; set; }
        public string Lang { get; set; }
        public string UserName { get; set; }

    }
    public class OvertimeApplicationMaintenance_Main
    {
        public string Factory { get; set; }
        public string Department_Code { get; set; }
        public string Department_Name { get; set; }
        public string Department_Code_Name { get; set; }
        public string USER_GUID { get; set; }
        public string Employee_Id { get; set; }
        public string Local_Full_Name { get; set; }
        public string Work_Shift_Type { get; set; }
        public string Work_Shift_Type_Name { get; set; }
        public string Work_Shift_Type_Str { get; set; }
        public string Overtime_Date { get; set; }
        public string Overtime_Date_Str { get; set; }
        public string Clock_In { get; set; }
        public string Clock_In_Str { get; set; }
        public string Clock_Out { get; set; }
        public string Clock_Out_Str { get; set; }
        public string Shift_Time_Str { get; set; }
        public string Overtime_Start { get; set; }
        public string Overtime_Start_Str { get; set; }
        public string Overtime_End { get; set; }
        public string Overtime_End_Str { get; set; }
        public string Apply_Time_Str { get; set; }
        public string Overtime_Hours { get; set; }
        public string Night_Hours { get; set; }
        public string Training_Hours { get; set; }
        public int Night_Eat_Times { get; set; }
        public string Update_By { get; set; }
        public DateTime Update_Time { get; set; }
        public string Update_Time_Str { get; set; }
    }
    public class OvertimeApplicationMaintenance_TypeheadKeyValue
    {
        public string Key { get; set; }
        public string USER_GUID { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public string Department_Name { get; set; }
    }
    public class OvertimeApplicationMaintenance_Table
    {
        public string Factory { get; set; }
        public DateTime Overtime_Date { get; set; }
        public string Employee_Id { get; set; }
        public string Work_Shift_Type { get; set; }
        public string Overtime_Start { get; set; }
        public string Overtime_End { get; set; }
        public string Overtime_Hours { get; set; }
        public string Night_Hours { get; set; }
        public string Training_Hours { get; set; }
        public string Night_Eat_Times { get; set; }
        public string Error_Message { get; set; }
    }
}