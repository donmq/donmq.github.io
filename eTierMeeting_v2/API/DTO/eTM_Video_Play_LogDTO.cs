using System;

namespace eTierV2_API.DTO
{
    public class eTM_Video_Play_LogDTO
    {
        public Guid Record_ID { get; set; }
        public string Center_Level { get; set; }
        public string Tier_Level { get; set; }
        public string Class_Level { get; set; }
        public string TU_ID { get; set; }
        public string deptId { get; set; }
        public string Page_Name { get; set; }
        public DateTime Play_Date_Time { get; set; }
    }
}