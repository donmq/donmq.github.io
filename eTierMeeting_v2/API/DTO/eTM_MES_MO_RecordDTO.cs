using System;

namespace eTierV2_API.DTO
{
    public class eTM_MES_MO_RecordDTO
    {
        public DateTime Data_Date { get; set; }
        public string Dept_ID { get; set; }
        public string MO_No { get; set; }
        public string MO_Seq { get; set; }
        public DateTime? Confirmed_Date { get; set; }
        public DateTime? Plan_Finish_Date { get; set; }
        public DateTime? Plan_Ship_Date { get; set; }
        public string Line_ID_ASY { get; set; }
        public string Line_ID_STC { get; set; }
        public string Style_No { get; set; }
        public string Style_Name { get; set; }
        public string Color_No { get; set; }
        public int Plan_Qty { get; set; }
        public int Output_Qty { get; set; }
        public int FGIN_Qty { get; set; }
        public string Nation { get; set; }
        public string Insert_By { get; set; }
        public DateTime Insert_Time { get; set; }
        public string Update_By { get; set; }
        public DateTime? Update_Time { get; set; }
        public int Balance { get; set; }
    }
}