using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eTierV2_API.Models
{
    public partial class eTM_Meeting_Log_Page
    {
        [Key]
        public Guid Record_ID { get; set; }
        [Required]
        [StringLength(10)]
        public string TU_ID { get; set; }
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
        [StringLength(20)]
        public string Page_Name { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime Start_Time { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? End_Time { get; set; }
        public bool? Click_Link { get; set; }
    }
}
