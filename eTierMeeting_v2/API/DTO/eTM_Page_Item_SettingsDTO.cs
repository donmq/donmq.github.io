using System;

namespace eTierV2_API.DTO
{
    public class eTM_Page_Item_SettingsDTO
    {
        public string Center_Level { get; set; }
        public string Tier_Level { get; set; }
        public string Class_Level { get; set; }
        public string Page_Name { get; set; }
        public string Item_ID { get; set; }
        public string Item_Name { get; set; }
        public string Item_Name_LL { get; set; }
        public decimal? Target { get; set; }
        public decimal? Tolerance { get; set; }
        public string Unit { get; set; }
        public string Update_By { get; set; }
        public DateTime Update_Time { get; set; }
        public bool Is_Active { get; set; }
    }
}