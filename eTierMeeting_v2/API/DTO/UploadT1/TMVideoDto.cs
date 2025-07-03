using System;

namespace eTierV2_API.DTO.UploadT1
{
    public class TMVideoDto
    {
        public string Video_Kind { get; set; } 
        public DateTime? Play_Date { get; set; }
        public short Seq { get; set; }
        public string Video_Title_ENG { get; set; }
        public string Video_Title_CHT { get; set; }
        public string VIdeo_Title_LCL { get; set; }
        public string Video_Icon_Path { get; set; }
        public string Video_Remark { get; set; }
        public string Center {get;set;}
        public string Tier {get;set;}
        public string Section {get;set;}
        public string Unit {get;set;}
        public string Unit_Name {get;set;}
        public DateTime? Insert_At {get;set;}
    }
}