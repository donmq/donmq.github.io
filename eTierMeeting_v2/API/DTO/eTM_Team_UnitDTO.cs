using System;

namespace eTierV2_API.DTO
{
    public class eTM_Team_UnitDTO
    {
        public string TU_ID { get; set; }
        public string TU_Name { get; set; }
        public string PDC { get; set; }
        public string Building { get; set; }
        public string Center_Level { get; set; }
        public string Tier_Level { get; set; }
        public string TU_Code { get; set; }
        public string Class1_Level { get; set; }
        public string Class2_Level { get; set; }
        public string Insert_By { get; set; }
        public DateTime Insert_Time { get; set; }
        public string Update_By { get; set; }
        public DateTime? Update_Time { get; set; }
    }
}