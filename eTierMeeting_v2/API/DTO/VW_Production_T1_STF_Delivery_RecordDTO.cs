using System;

namespace eTierV2_API.DTO
{
    public class VW_Production_T1_STF_Delivery_RecordDTO
    {
        public DateTime? Plan_Start_ASY { get; set; }
        public string Line_ID_ASY { get; set; }
        public string Line_ID_STF { get; set; }
        public string MO_No { get; set; }
        public string MO_Seq { get; set; }
        public string Style_Name { get; set; }
        public string Color_No { get; set; }
        public int? Plan_Qty { get; set; }
        public int? Output_Qty { get; set; }
        public int? Output_Balance { get; set; }
        public int? Transfer_Qty { get; set; }
        public int? Transfer_Balance { get; set; }
        public string Building { get; set; }
    }
}