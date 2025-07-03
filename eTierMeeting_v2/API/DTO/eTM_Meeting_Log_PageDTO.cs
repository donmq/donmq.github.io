using System;
namespace eTierV2_API.DTO
{
    public class eTM_Meeting_Log_PageDTO
    {
        public Guid Record_ID { get; set; }
        public string TU_ID { get; set; }
        public string deptId { get; set; }
        public string Center_Level { get; set; }
        public string Tier_Level { get; set; }
        public string Class_Level { get; set; }
        public string Page_Name { get; set; }
        public DateTime? Start_Time { get; set; }
        public DateTime? End_Time { get; set; }
        public bool? Click_Link { get; set; }
        public bool IsViewFirst { get; set; }

    }

    public class eTM_Meeting_Log_PageParamDTO
    {
        public Guid Record_ID { get; set; }
        public bool ClickLinkKaizenSystem { get; set; } = false;

    }
}