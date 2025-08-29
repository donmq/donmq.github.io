
namespace API.DTOs.AttendanceMaintenance
{
    public class OvertimeModificationMaintenanceParam
    {
        public string Factory { get; set; }
        public string Department { get; set; }
        public string Employee_ID { get; set; }
        public string Work_Shift_Type { get; set; }
        public string Overtime_Date_From { get; set; }
        public string Overtime_Date_To { get; set; }
        public string Language { get; set; }
        public string AttDate { get; set; }
    }

    public class OvertimeModificationMaintenanceDto
    {
        public string USER_GUID { get; set; }
        public string Factory { get; set; }
        public DateTime Overtime_Date { get; set; }
        public string Overtime_Date_Str { get; set; }
        public string Employee_ID { get; set; }
        public string Department_Code { get; set; }
        public string Department_Name { get; set; }
        public string Department_Code_Name { get; set; }
        public string Work_Shift_Type { get; set; }
        public string Work_Shift_Type_Name { get; set; }
        public string Overtime_Start { get; set; }
        public string Overtime_Start_Temp { get; set; }
        public string Overtime_End { get; set; }
        public string Overtime_End_Temp { get; set; }
        public string Overtime_Hours { get; set; }
        public string Night_Hours { get; set; }
        public string Night_Overtime_Hours { get; set; }
        public string Training_Hours { get; set; }
        public string Night_Eat_Times { get; set; }
        public string Holiday { get; set; }
        public string Holiday_Name { get; set; }
        public string Update_By { get; set; }
        public DateTime Update_Time { get; set; }
        public string Update_Time_Temp { get; set; }
        public string Local_Full_Name { get; set; }
        public string Work_Shift_Type_Time { get; set; }
        public string Clock_In_Time { get; set; }
        public string Clock_Out_Time { get; set; }
        public bool IsOvertimeDate { get; set; }
    }

    public class EmpPersonalAdd
    {
        public string USER_GUID { get; set; }
        public string Local_Full_Name { get; set; }
        public string Division { get; set; }
        public string Factory { get; set; }
        public string Employee_ID { get; set; }
        public string Department { get; set; }
        public string Department_Name { get; set; }
        public string Assigned_Division { get; set; }
        public string Assigned_Factory { get; set; }
        public string Assigned_Employee_ID { get; set; }
        public string Assigned_Department { get; set; }
        public string Work_Shift_Type { get; set; }
        public bool? Work8hours { get; set; }
        public string Employment_Status { get; set; }
    }

    public class ClockInClockOut
    {
        public string Work_Shift_Type_Time { get; set; }
        public string Clock_In_Time { get; set; }
        public string Clock_Out_Time { get; set; }
    }
}