using System.ComponentModel.DataAnnotations;

namespace eTierV2_API.Models
{
    public class VW_MES_4MReason
    {
        [Required]
        [StringLength(2)]
        public string Factory_ID { get; set; }
        [Required]
        [StringLength(1)]
        public string Reason_Type { get; set; }
        [Required]
        [StringLength(2)]
        public string Kind { get; set; }
        [Required]
        [StringLength(4)]
        public string Code { get; set; }
        [StringLength(100)]
        public string Desc_Local { get; set; }
        [StringLength(100)]
        public string Desc_TW { get; set; }
        [StringLength(100)]
        public string Desc_EN { get; set; }
        [Required]
        [StringLength(3)]
        public string PIC_ID { get; set; }
        public int? IsAction { get; set; }
        public int? IsUpload { get; set; }
    }
}