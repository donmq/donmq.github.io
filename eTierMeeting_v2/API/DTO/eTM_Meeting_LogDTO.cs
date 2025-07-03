using System;

namespace eTierV2_API.DTO
{
    public class eTM_Meeting_LogDTO
    {
        public Guid Record_ID { get; set; }
        public string TU_ID { get; set; }
        public DateTime Data_Date { get; set; }
        public DateTime Meeting_Start_Time { get; set; }
        public DateTime? Meeting_End_Time { get; set; }
        public string Record_Status { get; set; }
        public int? Duration_Sec { get; set; }
        public string Insert_By { get; set; }
        public DateTime Insert_Time { get; set; }
        public string Update_By { get; set; }
        public DateTime? Update_Time { get; set; }
    }
}