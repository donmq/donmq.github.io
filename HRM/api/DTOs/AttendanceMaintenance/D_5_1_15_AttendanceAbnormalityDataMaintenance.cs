namespace API.DTOs.AttendanceMaintenance
{
  public class AttendanceAbnormalityDataMaintenanceParam
  {
    public string Factory { get; set; }
    public string Department { get; set; }
    public string Employee_ID { get; set; }
    public string Work_Shift_Type { get; set; }
    public List<string> List_Attendance { get; set; }
    public string Reason_Code { get; set; }
    public string Att_Date_From { get; set; }
    public string Att_Date_From_Str { get; set; }
    public string Att_Date_To { get; set; }
    public string Att_Date_To_Str { get; set; }
    public string Lang { get; set; }
  }
  public class HRMS_Att_Temp_RecordDto
  {
    public string USER_GUID { get; set; }
    public string Factory { get; set; }
    public string Department_Code { get; set; }
    public string Department_Name { get; set; }
    public string Department_Code_Name { get; set; }
    public string Employee_ID { get; set; }
    public string Local_Full_Name { get; set; }
    public DateTime Att_Date { get; set; }
    public string Att_Date_Str { get; set; }
    public string Work_Shift_Type { get; set; }
    public string Work_Shift_Type_Str { get; set; }
    public string Leave_Code { get; set; }
    public string Leave_Code_Str { get; set; }
    public string Reason_Code { get; set; }
    public string Reason_Code_Str { get; set; }
    public string Clock_In { get; set; }
    public string Modified_Clock_In { get; set; }
    public string Clock_Out { get; set; }
    public string Modified_Clock_Out { get; set; }
    public string Overtime_ClockIn { get; set; }
    public string Modified_Overtime_ClockIn { get; set; }
    public string Overtime_ClockOut { get; set; }
    public string Modified_Overtime_ClockOut { get; set; }
    public string Days { get; set; }
    public string Holiday { get; set; }
    public string Holiday_Str { get; set; }
    public string Update_By { get; set; }
    public DateTime Update_Time { get; set; }
    public string Update_Time_Str { get; set; }

    public string Modified_Overtime_ClockOut_Old { get; set; }
    public string Modified_Overtime_ClockIn_Old { get; set; }
    public string Modified_Clock_In_Old { get; set; }
    public string Modified_Clock_Out_Old { get; set; }
  }

  public class EmployeeData
  {
    public string USER_GUID { get; set; }
    public string Local_Full_Name { get; set; }
    public string Department_Code { get; set; }
    public string Department_Code_Name { get; set; }
  }

}