using System;

namespace eTierV2_API.DTO
{
    public class VW_T2_Meeting_LogDTO
    {
        public DateTime Meeting_Date { get; set; }
        public string Level { get; set; }
        public string PDC { get; set; }
        public string Building { get; set; }
        public string TU_ID { get; set; }
        public decimal Plan_Hour { get; set; }
        public DateTime? Meeting_Start_Time { get; set; }
        public DateTime? Meeting_End_Time { get; set; }
        public decimal Duration_Sec { get; set; }
        public int Perform { get; set; }
        public int Effective { get; set; }
        public DateTime? Safety_Start_Time { get; set; }
        public DateTime? Safety_End_Time { get; set; }
        public decimal Safety_Duration { get; set; }
        public DateTime? Quality_Start_Time { get; set; }
        public DateTime? Quality_End_Time { get; set; }
        public decimal Quality_Duration { get; set; }
        public DateTime? Efficiency_Start_Time { get; set; }
        public DateTime? Efficiency_End_Time { get; set; }
        public decimal Efficiency_Duration { get; set; }
        public DateTime? Kaizen_Start_Time { get; set; }
        public DateTime? Kaizen_End_Time { get; set; }
        public decimal Kaizen_Duration { get; set; }
    }

    public class VW_T2_Meeting_LogSum
    {
        public int Perform_Total { get; set; }
        public int Perform_Total_Lines { get; set; }
        public int Perform_Score { get; set; }
        public int Effective_Total { get; set; }
        public int Effective_Total_Lines { get; set; }
        public int Effective_Score { get; set; }
    }

    public class VW_T2_Meeting_LogParam
    {
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
    }
}