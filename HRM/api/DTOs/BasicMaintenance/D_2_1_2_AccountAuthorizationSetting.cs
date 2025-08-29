namespace API.DTOs.BasicMaintenance
{
    public class AccountAuthorizationSetting_Base
    {
        public string Account { get; set; }
        public string Name { get; set; }
        public string Division { get; set; }
        public string Factory { get; set; }
        public string Department_ID { get; set; }
        public string ListRole_Str { get; set; }
        public List<string> ListRole { get; set; }
    }
    public class AccountAuthorizationSetting_Param : AccountAuthorizationSetting_Base
    {
        public int IsActive { get; set; }
        public string Lang { get; set; }
    }
    public class AccountAuthorizationSetting_Data : AccountAuthorizationSetting_Base
    {
        public bool IsActive { get; set; }
        public string IsActive_str { get; set; }
        public bool Password_Reset { get; set; }
        public string Update_By { get; set; }
        public DateTime Update_Time { get; set; }
        public string Update_Time_str { get; set; }
    }
}