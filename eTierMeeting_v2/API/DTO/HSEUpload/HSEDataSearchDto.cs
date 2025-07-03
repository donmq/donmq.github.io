using System;

namespace eTierV2_API.DTO.HSEUpload
{
    public class HSEDataSearchDto
    {
        public int HSE_Score_ID {get;set;}
        public string Center_Level { get; set; }
        public string Tier_Level { get; set; }
        public string Class_Level { get; set; }
        public string Building {get;set;}
        public string Line_Sname {get;set;}
        public string Dept_ID {get;set;}
        public string Evaluation {get;set;}
        public decimal Score {get;set;}
        public decimal? Target {get;set;}
        public string Update_By {get;set;}
        public DateTime? Update_Time {get;set;}
        public string Action {get;set;}
        public bool CheckImageAlert {get;set;}
    }
    public class HSESearchParam {
        public int Year {get;set;}
        public int Month {get;set;}
        public string Building {get;set;}
        public string DeptID {get;set;}
        public bool ClickImageAlert {get;set;}
    }
}