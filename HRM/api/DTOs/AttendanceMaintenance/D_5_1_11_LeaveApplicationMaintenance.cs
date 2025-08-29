
using SDCores;

namespace API.DTOs.AttendanceMaintenance
{
    public class LeaveApplicationMaintenance_Param
    {
        public string USER_GUID { get; set; }
        public string Factory { get; set; }
        public string Department { get; set; }
        public string Employee_Id { get; set; }
        public string Leave { get; set; }
        public string Leave_Date_From { get; set; }
        public string Leave_Date_From_Str { get; set; }
        public string Leave_Date_To { get; set; }
        public string Leave_Date_To_Str { get; set; }
        public string Lang { get; set; }
        public string UserName { get; set; }

    }
    public class LeaveApplicationMaintenance_Main
    {
        public string Factory { get; set; }
        public string Department_Code { get; set; }
        public string Department_Name { get; set; }
        public string Department_Code_Name { get; set; }
        public string USER_GUID { get; set; }
        public string Employee_Id { get; set; }
        public string Local_Full_Name { get; set; }
        public string Leave_Code { get; set; }
        public string Leave_Name { get; set; }
        public string Leave_Str { get; set; }
        public string Leave_Start { get; set; }
        public string Leave_Start_Str { get; set; }
        public string Min_Start { get; set; }
        public string Leave_End { get; set; }
        public string Leave_End_Str { get; set; }
        public string Min_End { get; set; }
        public string Days { get; set; }
        public string Update_By { get; set; }
        public DateTime Update_Time { get; set; }
        public string Update_Time_Str { get; set; }
    }
    public class LeaveApplicationMaintenance_TypeheadKeyValue
    {
        public string Key { get; set; }
        public string USER_GUID { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public string Department_Name { get; set; }
    }
    public class LeaveApplicationMaintenance_Table
    {
        public string Factory { get; set; }
        public string Employee_Id { get; set; }
        public string Leave_Code { get; set; }
        public string Leave_Start { get; set; }
        public string Min_Start { get; set; }
        public string Leave_End { get; set; }
        public string Min_End { get; set; }
        public string Days { get; set; }
        public string Error_Message { get; set; }
    }
}