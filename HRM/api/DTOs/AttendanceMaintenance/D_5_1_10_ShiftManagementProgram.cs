
namespace API.DTOs.AttendanceMaintenance
{
    public class ShiftManagementProgram_Param
    {
        public string Division { get; set; }
        public string USER_GUID { get; set; }
        public string Factory { get; set; }
        public string Department { get; set; }
        public string Employee_Id { get; set; }
        public string Work_Shift_Type_New { get; set; }
        public string Effective_Date { get; set; }
        public string Effective_Date_Str { get; set; }
        public string Lang { get; set; }
    }
    public class ShiftManagementProgram_Main
    {
        public string Division { get; set; }
        public string Factory { get; set; }
        public string USER_GUID { get; set; }
        public string Employee_Id { get; set; }
        public string Local_Full_Name { get; set; }
        public string Emp_Department { get; set; }
        public string Department { get; set; }
        public string Department_Name { get; set; }
        public string Work_Shift_Type_Old { get; set; }
        public string Work_Shift_Type_Old_Name { get; set; }
        public string Work_Shift_Type_New { get; set; }
        public string Work_Shift_Type_New_Name { get; set; }
        public string Effective_Date { get; set; }
        public string Effective_Date_Str { get; set; }
        public bool Effective_State { get; set; }
        public string Update_By { get; set; }
        public DateTime Update_Time { get; set; }
        public string Update_Time_Str { get; set; }
        public bool Is_Editable { get; set; }

    }
    public class ShiftManagementProgram_Update
    {
        public ShiftManagementProgram_Main Temp_Data { get; set; }
        public ShiftManagementProgram_Main Recent_Data { get; set; }
    }

    public class TypeheadKeyValue
    {
        public string Key { get; set; }
        public string USER_GUID { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public string Work_Shift_Type_Old { get; set; }
    }

}