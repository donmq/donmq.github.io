using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace eTierV2_API.Models
{
    public partial class eTM_Page_Settings
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
        [Required]
        [StringLength(100)]
        public string Link { get; set; }
        public int Seq { get; set; }
        public bool Is_Active { get; set; }
    }
}
