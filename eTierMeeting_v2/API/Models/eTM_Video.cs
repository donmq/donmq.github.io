using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eTierV2_API.Models
{
    public class eTM_Video
    {

        [Key]
        [Column(Order = 0)]
        public string Video_Kind { get; set; }
        [Key]
        [Column(Order = 1)]
        public string TU_ID { get; set; }
        [Key]
        [Column(Order = 2)]
        public DateTime Play_Date { get; set; }
        [Key]
        [Column(Order = 3)]
        public short Seq { get; set; }
        public string Video_Title_ENG { get; set; }
        public string Video_Title_CHT { get; set; }
        public string VIdeo_Title_LCL { get; set; }
        public string Video_Path { get; set; }
        public string Video_Icon_Path { get; set; }
        public string Video_Remark { get; set; }
        public string Insert_By { get; set; }
        public DateTime Insert_At { get; set; }
        public string Update_By { get; set; }
        public DateTime? Update_At { get; set; }
    }
}
