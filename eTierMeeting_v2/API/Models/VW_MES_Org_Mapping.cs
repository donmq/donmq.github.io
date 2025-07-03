using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eTierV2_API.Models
{
    public partial class VW_MES_Org_Mapping
    {
        [Required]
        [StringLength(1)]
        public string PDC_ID { get; set; }
        [StringLength(10)]
        public string Building { get; set; }
        [Required]
        [StringLength(3)]
        public string Line_ID { get; set; }
        [Required]
        [StringLength(4)]
        public string ASY { get; set; }
        [Required]
        [StringLength(4)]
        public string STC { get; set; }
        [Required]
        [StringLength(4)]
        public string STF { get; set; }
        [Required]
        [StringLength(4)]
        public string PRE { get; set; }
        [StringLength(3)]
        public string Line_ID_2 { get; set; }
        [StringLength(10)]
        public string Line_Sname { get; set; }
    }
}
