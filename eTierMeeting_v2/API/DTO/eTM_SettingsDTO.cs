using System;

namespace eTierV2_API.DTO
{
    public class eTM_SettingsDTO
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public string Insert_By { get; set; }
        public DateTime Insert_At { get; set; }
        public string Update_By { get; set; }
        public DateTime? Update_At { get; set; }
    }
}