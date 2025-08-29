
namespace API.DTOs.AttendanceMaintenance
{
    public class FemaleEmpMenstrualMain
    {
        public string USER_GUID { get; set; }
        public string Factory { get; set; }
        public DateTime Att_Month { get; set; }
        public string Att_Month_Str { get; set; }
        public DateTime Breaks_Date { get; set; }
        public string Breaks_Date_Str { get; set; }
        public string Employee_ID { get; set; }
        public string Local_Full_Name { get; set; }
        public string Department_Code { get; set; }
        public string Department_Name { get; set; }
        public string Department_Code_Name { get; set; }
        public DateTime Onboard_Date { get; set; }
        public string Onboard_Date_Str { get; set; }
        public string Time_Start { get; set; }
        public string Time_End { get; set; }
        public decimal Breaks_Hours { get; set; }
        public string Update_By { get; set; }
        public DateTime Update_Time { get; set; }
        public string Update_Time_Str { get; set; }
    }

    public class EmpMenstrualInfo
    {
        public string USER_GUID { get; set; }
        public string Local_Full_Name { get; set; }
        public string Department { get; set; }
        public string Department_Name { get; set; }
        public string Onboard_Date { get; set; }
    }

    public class FemaleEmpMenstrualParam
    {
        public string Factory { get; set; }
        public string Employee_ID { get; set; }
        public string Att_Month { get; set; }
        public string Att_Month_Str { get; set; }
        public string Department { get; set; }
        public string Language { get; set; }
    }

    public class EmpMenstrualInfoParam
    {
        public string Factory { get; set; }
        public string Employee_ID { get; set; }
        public string Language { get; set; }
        public string USER_GUID { get; set; }

    }
}