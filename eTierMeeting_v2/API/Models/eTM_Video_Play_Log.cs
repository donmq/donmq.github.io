using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eTierV2_API.Models
{
    public class eTM_Video_Play_Log
    {
        [Key]
        public Guid Record_ID { get; set; }
        [Required]
        [StringLength(20)]
        public string Center_Level { get; set; }
        [Required]
        [StringLength(3)]
        public string Tier_Level { get; set; }
        [Required]
        [StringLength(50)]
        public string Class_Level { get; set; }
        [Required]
        [StringLength(10)]
        public string TU_ID { get; set; }
        [Required]
        [StringLength(20)]
        public string Page_Name { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime Play_Date_Time { get; set; }
    }
}