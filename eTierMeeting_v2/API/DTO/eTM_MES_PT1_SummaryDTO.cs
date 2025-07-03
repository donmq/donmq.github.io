using System;

namespace eTierV2_API.DTO
{
    public class eTM_MES_PT1_SummaryDTO
    {
        public string Dept_ID { get; set; }
        public DateTime Data_Date { get; set; }
        public string Reason_Code { get; set; }
        public string In_Ex { get; set; }
        public int Impact_Qty { get; set; }
        public int Total_Impact_Qty { get; set; }
        public decimal Percentage
        {
            get
            {
                return Total_Impact_Qty == 0 ? 0 : Math.Round((decimal)Impact_Qty / (decimal)Total_Impact_Qty * 100, 2);
            }
        }
        public string Desc_Local { get; set; }
    }
}