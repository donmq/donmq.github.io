using System;
namespace eTierV2_API.DTO
{
    public class VW_Production_T1_UPF_Delivery_RecordDTO
    {
        public DateTime? Plan_End_STC { get; set; }
        public string Line_ID { get; set; }
        public string MO_No { get; set; }
        public string MO_Seq { get; set; }
        public string Model_Name { get; set; }
        public string Article { get; set; }
        public int? Plan_Qty { get; set; }
        public int? STC_Output { get; set; }
        public int? STC_FGIN { get; set; }
        public int? STC_Forward { get; set; }
        public int? Balance { get; set; }
        public string Building { get; set; }
    }
}