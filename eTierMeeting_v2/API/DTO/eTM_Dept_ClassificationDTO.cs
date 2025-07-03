using System;

namespace eTierV2_API.DTO
{
    public class eTM_Dept_ClassificationDTO
    {
        public string Class_Kind { get; set; }
        public string Dept_ID { get; set; }
        public string Dept_Name { get; set; }
        public string Class_Name { get; set; }
        public string Insert_By { get; set; }
        public DateTime Insert_At { get; set; }
        public string Update_By { get; set; }
        public DateTime? Update_At { get; set; }
        public eTM_Dept_ClassificationDTO()
        {
            this.Update_At = DateTime.Now;
        }
    }
}