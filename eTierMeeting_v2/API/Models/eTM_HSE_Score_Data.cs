using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eTierV2_API.Models
{
    public partial class eTM_HSE_Score_Data
    {
        [Key]
        public int HSE_Score_ID { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
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
        [StringLength(8)]
        public string TU_Code { get; set; }
        [Required]
        [StringLength(50)]
        public string Item_ID { get; set; }
        [Column(TypeName = "decimal(5, 2)")]
        public decimal Score { get; set; }
        [Required]
        [StringLength(50)]
        public string Update_By { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime Update_Time { get; set; }
    }
}