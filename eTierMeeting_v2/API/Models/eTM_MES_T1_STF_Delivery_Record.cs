using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace eTierV2_API.Models
{
    public partial class eTM_MES_T1_STF_Delivery_Record
    {
        [Key]
        [Column(TypeName = "datetime")]
        public DateTime Update_Date { get; set; }
        [Key]
        [StringLength(50)]
        public string MO_No { get; set; }
        [Key]
        [StringLength(50)]
        public string MO_Seq { get; set; }
        [Key]
        [StringLength(50)]
        public string Line_ID_STF { get; set; }
        [StringLength(50)]
        public string Line_ID_ASY { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? Plan_Start_ASY { get; set; }
        [StringLength(50)]
        public string Style_Name { get; set; }
        [StringLength(50)]
        public string Color_No { get; set; }
        public int? Plan_Qty { get; set; }
        public int? Output_Qty { get; set; }
        public int? Output_Balance { get; set; }
        public int? Transfer_Qty { get; set; }
        public int? Transfer_Balance { get; set; }
    }
}
