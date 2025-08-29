namespace API.DTOs.SystemMaintenance
{
    public class SystemLanguageSetting_Data
    {
        public string Language_Code { get; set; }
        public string Language_Name { get; set; }
        public bool IsActive { get; set; }
        public string Update_By { get; set; }
        public DateTime? Update_Time { get; set; }
    }
}