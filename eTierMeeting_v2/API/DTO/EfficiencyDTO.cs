using System;

namespace eTierV2_API.DTO
{
    public class EfficiencyDTO
    {
        public string Dept_ID { get; set; }
        public DateTime Data_Date { get; set; }
        public decimal? Target_Daily { get; set; }
        public decimal? Target_Monthly { get; set; }
        public decimal? Target_Target { get; set; }
        public decimal? Perform_Daily { get; set; }
        public decimal? Perform_Monthly { get; set; }
        public decimal? Perform_Target { get; set; }
        public decimal? EOLR_Daily { get; set; }
        public decimal? EOLR_Monthly { get; set; }
        public decimal? EOLR_Target { get; set; }
        public string Current_Date { get; set; }
        public string Line_Sname { get; set; }
    }
}