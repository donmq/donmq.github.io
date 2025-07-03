using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eTierV2_API.Models
{
    public class eTM_Settings
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Key { get; set; }
        [Required]
        [Column(TypeName = "text")]
        public string Value { get; set; }
        [Required]
        [StringLength(10)]
        public string Insert_By { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime Insert_At { get; set; }
        [StringLength(10)]
        public string Update_By { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? Update_At { get; set; }
    }
}