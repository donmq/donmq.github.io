using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace eTierV2_API.Models
{
    [Table("SM_Basic_Data")]
    public partial class SM_Basic_Data
    {
        [Key]
        [Required]
        public Guid UID { get; set; }
        [Required]
        [StringLength(200)]
        public string Basic_Class { get; set; }
        [Required]
        [StringLength(200)]
        public string Column_01 { get; set; }
        [StringLength(200)]
        public string Column_02 { get; set; }
        [StringLength(200)]
        public string Column_03 { get; set; }
        [StringLength(200)]
        public string Column_04 { get; set; }
        [StringLength(200)]
        public string Column_05 { get; set; }
        [StringLength(200)]
        public string Column_06 { get; set; }
        [StringLength(200)]
        public string Column_07 { get; set; }
        [StringLength(200)]
        public string Column_08 { get; set; }
        [StringLength(200)]
        public string Column_09 { get; set; }
        [StringLength(200)]
        public string Column_10 { get; set; }
        [Required]
        public DateTime Insert_At { get; set; }
        [Required]
        public string Insert_By { get; set; }
        public DateTime? Update_At { get; set; }
        public string Update_By { get; set; }
    }
}