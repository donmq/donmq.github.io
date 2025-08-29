namespace API.DTOs.AttendanceMaintenance
{
    public class HRMS_Att_Overtime_ParameterDTO
    {
        public string Division { get; set; }
        public string Factory { get; set; }
        public string Work_Shift_Type { get; set; }
        public string Work_Shift_Type_Name { get; set; }
        public string Effective_Month { get; set; }
        public string Overtime_Start { get; set; }
        public string Overtime_Start_Old { get; set; }
        public string Overtime_End { get; set; }
        public string Overtime_Hours { get; set; }
        public string Night_Hours { get; set; }
        public string Update_By { get; set; }
        public string Update_Time { get; set; }
    }
    public class HRMS_Att_Overtime_ParameterParam
    {
        public string Division { get; set; }
        public string Factory { get; set; }
        public string Work_Shift_Type { get; set; }
        public string Effective_Month { get; set; }
        public string Language { get; set; }
    }

    public class HRMS_Att_Overtime_ParameterUploadParam
    {
        public IFormFile File { get; set; }
    }
}