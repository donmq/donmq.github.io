using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eTierV2_API.Models
{
    public partial class eTM_Page_Item_Settings
    {
        [Key]
        [StringLength(20)]
        public string Center_Level { get; set; }
        [Key]
        [StringLength(3)]
        public string Tier_Level { get; set; }
        [Key]
        [StringLength(50)]
        public string Class_Level { get; set; }
        [Key]
        [StringLength(20)]
        public string Page_Name { get; set; }
        [Key]
        [StringLength(50)]
        public string Item_ID { get; set; }
        [Required]
        [StringLength(50)]
        public string Item_Name { get; set; }
        [Required]
        [StringLength(50)]
        public string Item_Name_LL { get; set; }
        [Column(TypeName = "decimal(5, 2)")]
        public decimal? Target { get; set; }
        [Column(TypeName = "decimal(5, 2)")]
        public decimal? Tolerance { get; set; }
        [StringLength(20)]
        public string Unit { get; set; }
        [Required]
        [StringLength(50)]
        public string Update_By { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime Update_Time { get; set; }
        public bool Is_Active { get; set; }
    }
}
