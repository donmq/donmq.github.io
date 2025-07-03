using System;

namespace eTierV2_API.DTO
{
    public class MES_Dept_TargetDTO
    {
        public string Factory_ID { get; set; }
        public short Year_Target { get; set; }
        public byte Month_Target { get; set; }
        public string Dept_ID { get; set; }
        public decimal? Output_Target { get; set; }
        public decimal? RFT_Target { get; set; }
        public decimal? Star_Target { get; set; }
        public decimal? Perform_Target { get; set; }
        public decimal? EOLR_Target { get; set; }
        public string Update_By { get; set; }
        public DateTime? Update_Time { get; set; }
    }
}