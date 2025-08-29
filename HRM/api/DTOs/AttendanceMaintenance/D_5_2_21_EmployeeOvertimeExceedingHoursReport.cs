namespace API.DTOs.AttendanceMaintenance
{
    public class EmployeeOvertimeExceedingHoursReportDto
    {
        public string Department_Code { get; set; }
        public string Department_Name { get; set; }
        public string Employee_ID { get; set; }
        public string Local_Full_Name { get; set; }
        public string Position_Title { get; set; }
        public string Work_Type { get; set; }
        public DateTime OnBoard_Date { get; set; }
        public decimal Overtime_Hours { get; set; }
        public decimal Period_Working_Hours { get; set; }
        public decimal First_Week { get; set; }
        public decimal Second_Week { get; set; }
        public decimal Third_Week { get; set; }
        public decimal Fourth_Week { get; set; }
        public decimal Fifth_Week { get; set; }
        public decimal Weekly_Working_Hours { get; set; }
    }

    public class EmployeeOvertimeExceedingHoursReportParam
    {
        public string Factory { get; set; }
        public string Start_Date { get; set; }
        public string End_Date { get; set; }
        public string Statistical_Method { get; set; }
        public int Abnormal_Overtime_Hours { get; set; }
        public string Language { get; set; }
        public string UserName { get; set; }
    }
}