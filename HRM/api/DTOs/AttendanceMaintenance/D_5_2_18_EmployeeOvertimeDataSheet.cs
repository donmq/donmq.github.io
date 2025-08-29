
namespace API.DTOs.AttendanceMaintenance
{
    public class EmployeeOvertimeDataSheetDto
    {
        public string Factory { get; set; }
        public string USER_GUID { get; set; }
        public string Department_Code { get; set; }
        public string Department_Name { get; set; }
        public string Employee_ID { get; set; }
        public string Local_Full_Name { get; set; }
        public string Work_Shift_Type { get; set; }
        public string Work_Shift_Type_Name { get; set; }
        public string Position_Title { get; set; }
        public string Position_Title_Name { get; set; }
        public string Work_Type { get; set; }
        public string Work_Type_Name { get; set; }
        public string Overtime_Date { get; set; }
        public decimal? Work_Hours { get; set; }
        public string Clock_In { get; set; }
        public string Clock_Out { get; set; }
        public decimal Overtime_Hour { get; set; }
        public decimal Training_Hours { get; set; }
        public decimal Night_Hours { get; set; }
        public decimal Night_Overtime_Hours { get; set; }
        public string Holiday { get; set; }
        public string Last_Clock_In_Time { get; set; }

    }

    public class EmployeeOvertimeDataSheetParam
    {
        public string Factory { get; set; }
        public string Employee_ID { get; set; }
        public string Department { get; set; }
        public string Language { get; set; }
        public string UserName { get; set; }
        public string Overtime_Date_Start { get; set; }
        public string Overtime_Date_End { get; set; }
    }
      public class EmployeeDepartmentDetails
    {
        public string USER_GUID { get; set; }
        public string Local_Full_Name { get; set; }
        public string Work_Type { get; set; }
        public string Position_Title { get; set; }
        public string Department_Code { get; set; }
        public string Department_Name { get; set; }

    }
    
}