using System;

namespace eTierV2_API.DTO
{
    public class eTM_T2_Meeting_SeetingDTO
    {
        public string Meeting_Date { get; set; }
        public string TU_ID { get; set; }
        public TimeSpan Start_Time { get; set; }
        public TimeSpan End_Time { get; set; }
        public string Update_By { get; set; }
        public DateTime Update_At { get; set; }
    }

    public class T2MeetingTimeSettingParam
    {
        public DateTime? Start_Date { get; set; }
        public DateTime? End_Date { get; set; }
        public string? Building_Or_Group { get; set; }
    }
}