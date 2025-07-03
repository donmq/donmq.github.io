using System;
using System.ComponentModel.DataAnnotations;

namespace eTierV2_API.Models
{
    public partial class eTM_HSE_Score_Image
    {
        [Key]
        public Guid HSE_Image_ID { get; set; }
        public int HSE_Score_ID { get; set; }
        [Required]
        [StringLength(100)]
        public string Image_Path { get; set; }
        [StringLength(255)]
        public string Remark { get; set; }
    }
}