using System;

namespace eTierV2_API.DTO
{
    public class eTM_MES_Quality_Defect_DataDTO
    {
        public string Data_Kind { get; set; }
        public string Dept_ID { get; set; }
        public DateTime Data_Date { get; set; }
        public string Reason_ID { get; set; }
        public decimal Finding_Qty { get; set; }
        public string Image_Path { get; set; }
        public string Insert_By { get; set; }
        public DateTime Insert_Time { get; set; }
        public string Update_By { get; set; }
        public DateTime? Update_Time { get; set; }
    }
}