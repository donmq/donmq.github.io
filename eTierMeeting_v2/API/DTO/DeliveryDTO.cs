using System;

namespace eTierV2_API.DTO
{
    public class DeliveryDTO
    {
        public string Comfirmed_Date { get; set; }
        public string Plan_Finish_Date { get; set; }
        public string Plan_Ship_Date { get; set; }
        public DateTime? Plan_Ship_Date_N { get; set; }
        public string Line_ID_ASY { get; set; }
        public string Line_ID_STC { get; set; }
        public string MO_No { get; set; }
        public string MO_Seq { get; set; }
        public string Model_Name { get; set; }
        public string Article { get; set; }
        public decimal? Plan_Qty { get; set; }
        public int UTN_Yield_Qty { get; set; }
        public decimal? IN_Qty { get; set; }
        public int? QTY { get; set; }
        public string Nation { get; set; }
        public string BG_Color { get => GetBGColor(Plan_Finish_Date); }
        public string Building { get; set; }
        public string PDC_ID { get; set; }
        public string BG_Color2 { get; set; }
        public string BG_Color3 { get; set; }
        public string Dept_ID { get; set; }

        private static string GetBGColor(string Plan_Finish_Date)
        {
            var date = Convert.ToDateTime(Plan_Finish_Date).Date;
            double totalDays = (date - DateTime.Now.Date).TotalDays;
            string result = totalDays >= 3 ? "green" : totalDays == 2 ? "yellow" : "red";

            return result;
        }
    }
}