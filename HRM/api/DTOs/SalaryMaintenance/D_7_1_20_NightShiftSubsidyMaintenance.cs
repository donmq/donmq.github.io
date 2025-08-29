namespace API.DTOs.SalaryMaintenance
{
    public class NightShiftSubsidyMaintenanceDto_Param
    {
        public string Factory { get; set; }
        public string Year_Month { get; set; }
        public bool Is_Delete { get; set; }
        public string Employee_ID {get; set;}
        public List<string> Permission {get; set;}
    }

    public class Query_HRMS_Sal_AddDedItem_Values{
        public string Factory { get; set; }
        public DateTime Year_Month { get; set; }
        public string Permission {get; set;}
        public string AddDed_Type { get; set; }
        public string AddDed_Item { get; set; } 
        public string Salary_Type { get; set; }
    }

    public class HRMS_Sal_AddDedItem_Monthly_Data
    {
        public string USER_GUID {get; set;}
        public string Employee_ID {get; set;}
        public string PermissionGroup {get; set;}
    }
}