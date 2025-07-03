using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace eTierV2_API.Models
{
    [Keyless]
    public partial class VW_LineGroup
    {
        [Required]
        [StringLength(1)]
        public string Factory_ID { get; set; }
        [Required]
        [StringLength(5)]
        public string Dept_ID { get; set; }
        [Required]
        [StringLength(1)]
        public string Kind { get; set; }
        [StringLength(5)]
        public string Building { get; set; }
        [StringLength(5)]
        public string Line_Group { get; set; }
    }
}
