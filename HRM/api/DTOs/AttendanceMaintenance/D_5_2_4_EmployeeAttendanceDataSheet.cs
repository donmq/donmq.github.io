
namespace API.DTOs.AttendanceMaintenance
{
    public class EmployeeAttendanceDataSheetDto
    {
        public string Department { get; set; }
        public string Department_Name { get; set; }
        public string Employee_ID { get; set; }
        public string LocalFullName { get; set; }
        public string Work_Shift_Type { get; set; }
        public string Work_Shift_Type_Name { get; set; }
        //HRMS_Att_Change_Record.Leave_Code
        public string Attendance { get; set; }
        public string Attendance_Name { get; set; }
        public string Clock_In { get; set; }
        public string Clock_Out { get; set; }
        public string Overtime_ClockIn { get; set; }
        public string Overtime_ClockOut { get; set; }
        public decimal? DelayHour { get; set; }
        public decimal? WorkHour { get; set; }
        public decimal? NormalOvertime { get; set; }
        public decimal? TrainingOvertime { get; set; }
        public decimal? HolidayOvertime { get; set; }
        public decimal? Night { get; set; }
        public decimal? NightOvertime { get; set; }
        public decimal? Total { get; set; }
        public string Holiday { get; set; }
        public DateTime? Att_Date { get; set; }
        public bool Swipe_Card_Option { get; set; }

        
    }

    public class EmployeeAttendanceDataSheetParam
    {
        public string Factory { get; set; }
        public string Att_Date_From { get; set; }
        public string Att_Date_To { get; set; }
        public string Work_Shift_Type { get; set; }
        public string Employee_ID { get; set; }
        public string Department { get; set; }
        public string language { get; set; }
    }
    public class BasicCodeType
    {
        public string Code { get; set; }
        public string Name { get; set; }
       
    }
}