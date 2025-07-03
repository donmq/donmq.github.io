using System;

namespace eTierV2_API.DTO
{
    public class DefectTop3DTO
    {
        public DateTime Data_Date { get; set; }
        public string Def_DescVN { get; set; }
        public string Reason_ID { get; set; }
        public decimal Finding_Qty { get; set; }
        public string Image_Path { get; set; }
    }
}