using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs.SalaryMaintenance
{
    public class FinSalaryCloseMaintenance_MainData
    {
        public string Factory { get; set; }
        public DateTime Year_Month { get; set; }
        public string Year_Month_Str { get; set; }
        public string Department { get; set; }
        public string Department_Name { get; set; }
        public string Employee_ID { get; set; }
        public string Local_Full_Name { get; set; }
        public string Permission_Group { get; set; }
        public string Close_Status { get; set; }
        public DateTime? Close_End { get; set; }
        public string Close_End_Str { get; set; }

        public DateTime Onboard_Date { get; set; }
        public string Onboard_Date_Str { get; set; }

        public DateTime? Resign_Date { get; set; }
        public string Resign_Date_Str { get; set; }

        public string Update_By { get; set; }
        public string Update_Time { get; set; }

    }
    public class FinSalaryCloseMaintenance_Temp
    {
        public string USER_GUID { get; set; }
        public string Factory { get; set; }
        public DateTime Sal_Month { get; set; }
        public string Department { get; set; }
        public string Employee_ID { get; set; }
        public string Permission_Group { get; set; }
        public string Close_Status { get; set; }
        public DateTime? Close_End { get; set; }
        public DateTime Onboard_Date { get; set; }
        public string Update_By { get; set; }
        public DateTime Update_Time { get; set; }

    }
    public class FinSalaryCloseMaintenance_Param
    {
        public string Factory { get; set; }
        public string Year_Month { get; set; }
        public string Kind { get; set; }
        public string Department { get; set; }
        public string Employee_ID { get; set; }
        public List<string> Permission_Group { get; set; }
        public string Language { get; set; }
    }

    public class FinSalaryCloseMaintenance_UpdateParam
    {
        public string Factory { get; set; }
        public string Year_Month { get; set; }
        public string Employee_ID { get; set; }
        public string Close_Status { get; set; }
        public string Update_By { get; set; }
    }

    public class BatchUpdateData
    {
        public string Employee_ID {get; set;}
        public string Department {get; set;}

    }
    public class BatchUpdateData_Param
    {
        public string Factory { get; set; }
        public string Year_Month { get; set; }
        public List<string> Permission_Group { get; set; }
        public string Close_Status { get; set; }
        public string Kind { get; set; }
    }
}