
using API.Models;

namespace API.DTOs.CompulsoryInsuranceManagement
{
    public class CompulsoryInsuranceDataMaintenanceDto
    {
        public string USER_GUID { get; set; }
        public string Factory { get; set; }
        public string Employee_ID { get; set; }
        public string Local_Full_Name { get; set; }
        public string Insurance_Type { get; set; }
        public string Insurance_Type_Name { get; set; }
        public string Insurance_Start { get; set; }
        public string Insurance_End { get; set; }
        public string Insurance_Num { get; set; }
        public string Update_By { get; set; }
        public string Update_Time { get; set; }
    }
    public class CompulsoryInsuranceDataMaintenanceParam
    {
        public string Factory { get; set; }
        public string Employee_ID { get; set; }
        public string Insurance_Type { get; set; }
        public string Insurance_Start { get; set; }
        public string Insurance_End { get; set; }
        public string Language { get; set; }
        public string SearchMethod { get; set; }

    }

    public class CompulsoryInsuranceDataMaintenance_Personal
    {
        public string USER_GUID { get; set; }
        public string Local_Full_Name { get; set; }
    }

    public class CompulsoryInsuranceDataMaintenance_Upload
    {
        public IFormFile File { get; set; }
        public string Language { get; set; }
    }

    public class CompulsoryInsuranceDataMaintenance_Report : HRMS_Ins_Emp_Maintain
    {
        public string Error_Message { get; set; }
        public string Create_Date_Str { get; set; }
        public string IsCorrect { get; set; }
    }
}

