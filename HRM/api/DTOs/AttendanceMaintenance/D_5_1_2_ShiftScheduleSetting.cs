namespace API.DTOs.AttendanceMaintenance
{
    public class HRMS_Att_Work_ShiftDto
    {
        public string Division { get; set; }
        public string Factory { get; set; }
        public string Work_Shift_Type { get; set; }
        public string Work_Shift_Type_Title { get; set; }
        public int Week { get; set; }
        public string Clock_In { get; set; }
        public string Clock_Out { get; set; }
        public string Overtime_ClockIn { get; set; }
        public string Overtime_ClockOut { get; set; }
        public string Lunch_Start { get; set; }
        public string Lunch_End { get; set; }
        public string Overnight { get; set; }
        public string Work_Hours { get; set; }
        public string Work_Days { get; set; }
        public bool Effective_State { get; set; }
        public string Update_By { get; set; }
        public DateTime Update_Time { get; set; }
        public string Update_Time_Str { get; set; }
    }

    public class HRMS_Att_Work_ShiftParam
    {
        public string Division { get; set; }
        public string Factory { get; set; }
        public string Work_Shift_Type { get; set; }
        public string Language { get; set; }
    }
}