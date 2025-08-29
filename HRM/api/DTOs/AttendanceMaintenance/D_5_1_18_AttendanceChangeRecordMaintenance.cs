
namespace API.DTOs.AttendanceMaintenance;

public partial class HRMS_Att_Change_RecordDto
{
    public string USER_GUID { get; set; }
    public string Factory { get; set; }
    public string Department_Code { get; set; }
    public string Department_Name { get; set; }
    public string Department_Code_Name { get; set; }
    public string Employee_ID { get; set; }
    public string Local_Full_Name { get; set; }
    public DateTime Att_Date { get; set; } // Date
    public string Att_Date_Str { get; set; } // Date
    public string Work_Shift_Type { get; set; }
    public string Work_Shift_Type_Str { get; set; }
    public string Leave_Code { get; set; } // Attendance
    public string Leave_Code_Str { get; set; } // Attendance
    public string Before_Leave_Code { get; set; } // temp for edit
    public string After_Leave_Code { get; set; } // temp for edit
    public string Reason_Code { get; set; } // Update Reason
    public string Reason_Code_Str { get; set; } // Update Reason
    public string Clock_In { get; set; } // from HRMS_Att_Change_Reason
    public string Modified_Clock_In { get; set; } // Clock_In
    public string Clock_Out { get; set; }
    public string Modified_Clock_Out { get; set; }
    public string Overtime_ClockIn { get; set; }
    public string Modified_Overtime_ClockIn { get; set; }
    public string Overtime_ClockOut { get; set; }
    public string Modified_Overtime_ClockOut { get; set; }
    public decimal Days { get; set; }
    public decimal Before_Days { get; set; } // temp for edit
    public decimal After_Days { get; set; } // temp for edit
    public string Holiday { get; set; }
    public string Holiday_Str { get; set; }
    public bool isEdit { get; set; }
    public string Update_By { get; set; }
    public DateTime Update_Time { get; set; }
    public string Update_Time_Str { get; set; }
    public bool IsAttDate { get; set; }
}

public class HRMS_Att_Change_Record_Params
{
    public string Lang { get; set; }
    public string Factory { get; set; }
    public string Employee_ID { get; set; }
    public string Department { get; set; }
    public string Work_Shift_Type { get; set; }
    public string Leave_Code { get; set; } // Attendance
    public string Reason_Code { get; set; } // Update_Reason
    public string Date_Start { get; set; }
    public string Date_Start_Str { get; set; }
    public string Date_End { get; set; }
    public string Date_End_Str { get; set; }

}

public class HRMS_Att_Change_Record_Delete_Params
{
    public string USER_GUID { get; set; }
    public string Lang { get; set; }
    public string Factory { get; set; }
    public string Att_Date { get; set; }
    public string Employee_ID { get; set; }
    public decimal Days { get; set; }

}

public class EmployeeDataChange
{
    public string USER_GUID { get; set; }
    public string Local_Full_Name { get; set; }
    public string Department_Code { get; set; }
    public string Department_Code_Name { get; set; }
}
public partial class HRMS_Att_Change_ReasonDto
{
    public string USER_GUID { get; set; }
    public string Factory { get; set; }
    public DateTime Att_Date { get; set; }
    public string Employee_ID { get; set; }
    public string Work_Shift_Type { get; set; }
    public string Leave_Code { get; set; }
    public string Reason_Code { get; set; }
    public string Clock_In { get; set; }
    public string Clock_Out { get; set; }
    public string Overtime_ClockIn { get; set; }
    public string Overtime_ClockOut { get; set; }
    public string Update_By { get; set; }
    public DateTime Update_Time { get; set; }
}