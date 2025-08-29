using System.ComponentModel.DataAnnotations;

namespace API.DTOs.AttendanceMaintenance;

public partial class Leave_Record_Modification_MaintenanceDto
{

    [Required]
    public string USER_GUID { get; set; }

    public string Factory { get; set; }
    public string Department_Code { get; set; }
    public string Department_Name { get; set; }
    public string Department_Code_Name { get; set; }
    public string Employee_ID { get; set; }
    public string Local_Full_Name { get; set; }

    [Required]
    [StringLength(10)]
    public string Work_Shift_Type { get; set; }
    public string Work_Shift_Type_Str { get; set; }

    public string Leave_Code { get; set; }
    public string Leave_Code_Str { get; set; }

    public DateTime? Leave_Date { get; set; }
    public string Leave_Date_Str { get; set; }

    public decimal Days { get; set; }
    public bool isEdit { get; set; }
    public bool IsLeaveDate { get; set; }

    [Required]
    public string Update_By { get; set; }
    public DateTime? Update_Time { get; set; }
    public string Update_Time_Str { get; set; }
}

public partial class Leave_Record_Modification_MaintenanceSearchParamDto
{
    public string Lang { get; set; }
    public string Factory { get; set; }
    public string Department { get; set; }
    public string Employee_ID { get; set; }
    public string Permission_Group { get; set; }
    public string Leave { get; set; } // HRMS_Att_Leave_Maintain.Leave_End
    public string Date_Start { get; set; }
    public string Date_Start_Str { get; set; }
    public string Date_End { get; set; }
    public string Date_End_Str { get; set; }
    public string Work_Shift_Type { get; set; }
    public string Leave_Date_Str { get; set; }
}

public class Leave_Record_Modification_Maintenance_Delete_Params
{
    public string USER_GUID { get; set; }
    public string Leave_code { get; set; }
    public string Lang { get; set; }
    public string Factory { get; set; }
    public string Leave_Date_Str { get; set; }
    public string Work_Shift_Type { get; set; }
    public string Employee_ID { get; set; }
    public string Update_By { get; set; }

}
