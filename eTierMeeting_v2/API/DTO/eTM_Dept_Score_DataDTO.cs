using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace eTierV2_API.DTO
{
    public class eTM_Dept_Score_DataDTO
    {
        public string Dept_ID { get; set; }
        public DateTime Data_Date { get; set; }
        public decimal? RFT { get; set; }
        public decimal? BA { get; set; }
        public decimal? Dept_Output_Target { get; set; }
        public decimal? Output_FGIN { get; set; }
        public decimal? Defect_Qty { get; set; }
        public decimal? Impact_Qty_4M { get; set; }
        public decimal? Actual_Working_Hrs { get; set; }
        public string Insert_By { get; set; }
        public DateTime Insert_Time { get; set; }
        public string Update_By { get; set; }
        public DateTime? Update_Time { get; set; }
    }
}