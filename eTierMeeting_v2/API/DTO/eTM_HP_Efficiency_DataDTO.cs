using System;

namespace eTierV2_API.DTO
{
    public class eTM_HP_Efficiency_DataDTO
    {
        public DateTime Data_Date { get; set; }
        public string Dept_ID { get; set; }
        public int? Actual_Qty { get; set; }
        public int? Impact_Qty { get; set; }
        public int? Target_Qty { get; set; }
        public decimal? Hour_Base { get; set; }
        public decimal? Hour_Overtime { get; set; }
        public decimal? Hour_IE { get; set; }
        public decimal? Hour_Tot { get; set; }
        public decimal? Hour_Ex { get; set; }
        public decimal? Hour_Learn { get; set; }
        public decimal? Hour_Transfer { get; set; }
        public decimal? Hour_In { get; set; }
        public decimal? Hour_Out { get; set; }
    }
}