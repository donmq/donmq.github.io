
namespace API.DTOs.SalaryMaintenance
{
    public class D_7_10_SalaryItemToAccountingCodeMappingMaintenanceDto
    {
        public string Factory { get; set; }
        public string Salary_Item { get; set; }
        public string Salary_Item_Name { get; set; }
        public string Main_Acc { get; set; }
        public string Sub_Acc { get; set; }
        public string DC_Code { get; set; }
        public string Update_By { get; set; }
        public DateTime? Update_Time { get; set; }
        public string Update_Time_Str { get; set; }
    }

    public class D_7_10_SalaryItemToAccountingCodeMappingMaintenanceParam
    {
        public string Factory { get; set; }
        public string Language { get; set; }
    }


}