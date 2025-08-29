using API.Models;

namespace API.DTOs.AttendanceMaintenance
{
    public class DailyUnregisteredOvertimeList_Param
    {
        public string Factory { get; set; }
        public string Department { get; set; }
        public string Employee_Id { get; set; }
        public string Clock_Out_Date_From { get; set; }
        public string Clock_Out_Date_To { get; set; }
        public string Clock_Out_Date_From_Str { get; set; }
        public string Clock_Out_Date_To_Str { get; set; }
        public string Lang { get; set; }
    }
    public class DailyUnregisteredOvertimeList_Table
    {
        public string Department_Code { get; set; }
        public string Department_Name { get; set; }
        public string Employee_Id { get; set; }
        public string Local_Full_Name { get; set; }
        public string Date { get; set; }
        public string Work_Shift_Type { get; set; }
        public string Leave { get; set; }
        public string Clock_In { get; set; }
        public string Clock_Out { get; set; }
    }
    public class DailyUnregisteredOvertimeList_HACR_DTO : HRMS_Att_Change_Record
    {
        public string Local_Full_Name { get; set; }
        public string Factory_HEP { get; set; }
        public string Department_HEP { get; set; }
        public string Division_HEP { get; set; }
    }
}