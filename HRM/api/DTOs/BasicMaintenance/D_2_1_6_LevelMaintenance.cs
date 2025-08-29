namespace API.DTOs.BasicMaintenance
{
    public class LevelMaintenanceParam
    {
        public string Type_Code { get; set; }
        public string Level { get; set; }
        public string Level_Code { get; set; }
        public string Language { get; set; }
    }
    public class HRMS_Basic_LevelDto
    {
        public decimal Level { get; set; }
        public string Level_Code { get; set; }
        public string Level_Code_Name { get; set; }
        public string Type_Code { get; set; }
        public bool IsActive { get; set; }
        public string Type_Code_Name { get; set; }
        public string Status { get; set; }
        public string Update_By { get; set; }
        public DateTime? Update_Time { get; set; }
    }
}