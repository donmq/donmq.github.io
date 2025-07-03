using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace eTierV2_API.Models
{
    [Keyless]
    public partial class VW_Production_T1_STF_Delivery_Record
    {
        [Column(TypeName = "datetime")]
        public DateTime? Plan_Start_ASY { get; set; }
        [StringLength(3)]
        public string Line_ID_ASY { get; set; }
        [StringLength(3)]
        public string Line_ID_STF { get; set; }
        [StringLength(15)]
        public string MO_No { get; set; }
        [StringLength(3)]
        public string MO_Seq { get; set; }
        [StringLength(40)]
        public string Style_Name { get; set; }
        [StringLength(10)]
        public string Color_No { get; set; }
        public int? Plan_Qty { get; set; }
        public int? Output_Qty { get; set; }
        public int? Output_Balance { get; set; }
        public int? Transfer_Qty { get; set; }
        public int? Transfer_Balance { get; set; }
        [StringLength(10)]
        public string Building { get; set; }
    }
}
