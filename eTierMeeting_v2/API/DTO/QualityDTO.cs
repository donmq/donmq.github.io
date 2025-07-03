using System;

namespace eTierV2_API.DTO
{
    public class QualityDTO
    {
        public string Dept_Name { get; set; }
        public DateTime Data_Date { get; set; }
        public decimal? RFT_Target { get; set; }
        public decimal? Star_Target { get; set; }
        public decimal? RFT { get; set; }
        public decimal? BA { get; set; }
        public string Reason_ID { get; set; }
        public string Dept_Desc { get; set; }
        public decimal Finding_Qty { get; set; }
        public string Image_Path { get; set; }

        public string ColoRft { get; set; }
        public string ColoBa { get; set; }
    }
}