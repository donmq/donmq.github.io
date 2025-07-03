using System;

namespace eTierV2_API.DTO.HSEUpload
{
    public class HSEScoreFromMesDto
    {
        public string Center_Level { get; set; }
        public string Tier_Level { get; set; }
        public string Class_Level { get; set; }
        public string Line_Sname {get;set;}
        public string Building {get;set;}
        public string Dept_ID { get; set; }
        public string Item_ID { get; set; }
        public string Update_By { get; set; }
        public DateTime? Update_Time { get; set; }
    }
    public class DataDownloadTemplate {
        public int Year {get;set;}
        public int Month {get;set;}
        public string Center_Level {get;set;}
        public string Tier_Level {get;set;}
        public string Section {get;set;}
        public string Building {get;set;}
        public string Line_Sname {get;set;}
        public string TU_Code {get;set;}
    }
}