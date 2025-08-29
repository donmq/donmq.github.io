namespace API.DTOs.BasicMaintenance
{
    public class CodeMaintenanceParam
    {
        public string Type_Seq { get; set; }
        public string Type_Name { get; set; }
        public string Code { get; set; }
        public string Code_Name { get; set; }
    }
    public class HRMS_Basic_CodeDto
    {
        public string Type_Seq { get; set; }
        public string Code { get; set; }
        public string Code_Name { get; set; }
        public string Char1 { get; set; }
        public string Char2 { get; set; }
        public DateTime? Date1 { get; set; }
        public DateTime? Date2 { get; set; }
        public DateTime? Date3 { get; set; }
        public int? Int1 { get; set; }
        public int? Int2 { get; set; }
        public int? Int3 { get; set; }
        public decimal? Decimal1 { get; set; }
        public decimal? Decimal2 { get; set; }
        public decimal? Decimal3 { get; set; }
        public string Remark { get; set; }
        public string Remark_Code { get; set; }
        public bool IsActive { get; set; }
        public string Update_By { get; set; }
        public DateTime? Update_Time { get; set; }
        public string Seq { get; set; }



        // BasicCode.Type_Name
        public string Type_Name { get; set; }
        public string State { get; set; }
        public string Update_Time_String { get; set; }

    }
}