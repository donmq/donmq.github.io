
namespace API.DTOs.AttendanceMaintenance
{
    public class HRMS_Att_Overtime_TempDto
    {
        public string USER_GUID { get; set; }
        public string Factory { get; set; }
        public string Department_Code { get; set; }
        public string Department_Name { get; set; }
        public string Department_Code_Name { get; set; }
        public string Employee_ID { get; set; }
        public string Local_Full_Name { get; set; }
        public DateTime Date { get; set; }
        public string Date_Str { get; set; }
        public string Work_Shift_Type { get; set; }
        public string Work_Shift_Type_Str { get; set; }
        public string Shift_Time { get; set; }
        public string Clock_In_Time { get; set; }
        public string Clock_Out_Time { get; set; }
        public string Overtime_Start { get; set; }
        public string Overtime_End { get; set; }
        public decimal Overtime_Hours { get; set; }
        public decimal Night_Hours { get; set; }
        public decimal Night_Overtime_Hours { get; set; }
        public decimal Training_Hours { get; set; }
        public int Night_Eat { get; set; }
        public string Holiday { get; set; }
        public string Holiday_Str { get; set; }
        public string Update_By { get; set; }
        public DateTime Update_Time { get; set; }
        public string Update_Time_Str { get; set; }
    }

    public class HRMS_Att_Overtime_TempParam
    {
        public string Factory { get; set; }
        public string Department { get; set; }
        public string Employee_ID { get; set; }
        public string Shift { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public string lang { get; set; }
    }

    public class OvertimeTempPersonal
    {
        public string USER_GUID { get; set; }
        public string Local_Full_Name { get; set; }
        public string Department_Code { get; set; }
        public string Department_Code_Name { get; set; }
    }

    public class OvertimeTempPersonalParam
    {
        public string Factory { get; set; }
        public string EmployeeID { get; set; }
        public string Date { get; set; }
        public string Lang { get; set; }
    }
    public class ClockInOutTempRecord
    {
        public string Clock_In_Time { get; set; }
        public string Clock_Out_Time { get; set; }
    }

    public class DepartmentInfo
    {
        public string Name { get; set; }
        public string CodeName { get; set; }
    }
}