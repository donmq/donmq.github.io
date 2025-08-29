namespace API.DTOs.SalaryMaintenance
{
    public class AdditionDeductionItemToAccountingCodeMappingMaintenanceDto
    {
        public string Factory { get; set; }
        public string AddDed_Item { get; set; }
        public string AddDed_Item_Title { get; set; }
        public string Main_Acc { get; set; }
        public string Sub_Acc { get; set; }
        public string Update_By { get; set; }
        public DateTime Update_Time { get; set; }
        public string Update_Time_Str { get; set; }
    }

    public class AdditionDeductionItemToAccountingCodeMappingMaintenanceParam
    {
        public string Factory { get; set; }

        public string Language { get; set; }
    }
}