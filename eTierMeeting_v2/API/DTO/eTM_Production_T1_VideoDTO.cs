using System;

namespace eTierV2_API.DTO
{
    public class eTM_Production_T1_VideoDTO
    {
        public string Video_Kind { get; set; }
        public string Dept_ID { get; set; }
        public DateTime Play_Date { get; set; }
        public short Seq { get; set; }
        public string Video_Title_ENG { get; set; }
        public string Video_Title_CHT { get; set; }
        public string VIdeo_Title_LCL { get; set; }
        public string Video_Path { get; set; }
        public string Video_Icon_Path { get; set; }
        public string Video_Remark { get; set; }
        public string Insert_By { get; set; }
        public DateTime Insert_At { get; set; }
        public string Update_By { get; set; }
        public DateTime? Update_At { get; set; }
    }
}