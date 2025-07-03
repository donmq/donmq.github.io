using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eTierV2_API.Models
{
    public partial class VW_KanBan_POList_V2
    {
        [StringLength(5)]
        public string Comfirmed_Date { get; set; }
        [StringLength(5)]
        public string Plan_Finish_Date { get; set; }
        [StringLength(5)]
        public string Plan_Ship_Date { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? Plan_Ship_Date_N { get; set; }
        [StringLength(3)]
        public string Line_ID_ASY { get; set; }
        [Required]
        [StringLength(3)]
        public string Line_ID_STC { get; set; }
        [StringLength(15)]
        public string MO_No { get; set; }
        [StringLength(3)]
        public string MO_Seq { get; set; }
        [StringLength(50)]
        public string Model_Name { get; set; }
        [StringLength(50)]
        public string Article { get; set; }
        public int Plan_Qty { get; set; }
        public int UTN_Yield_Qty { get; set; }
        [Column(TypeName = "numeric(38, 0)")]
        public decimal? IN_Qty { get; set; }
        [Column(TypeName = "numeric(38, 0)")]
        public decimal? QTY { get; set; }
        [StringLength(50)]
        public string Nation { get; set; }
        [StringLength(6)]
        public string BG_Color { get; set; }
        [StringLength(10)]
        public string Building { get; set; }
        [Required]
        [StringLength(1)]
        public string PDC_ID { get; set; }
        [Required]
        [StringLength(6)]
        public string BG_Color2 { get; set; }
        [Required]
        [StringLength(6)]
        public string BG_Color3 { get; set; }
        [StringLength(4)]
        public string Dept_ID { get; set; }
    }
}
