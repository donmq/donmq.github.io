using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
namespace eTierV2_API.Models
{
    [Keyless]
    public partial class VW_Production_T1_UPF_Delivery_Record
    {
        [Column(TypeName = "datetime")]
        public DateTime? Plan_End_STC { get; set; }
        [StringLength(3)]
        public string Line_ID { get; set; }
        [StringLength(3)]
        public string MO_No { get; set; }
        [StringLength(3)]
        public string MO_Seq { get; set; }
        [StringLength(40)]
        public string Model_Name { get; set; }
        [StringLength(10)]
        public string Article { get; set; }
        public int? Plan_Qty { get; set; }
        public int? STC_Output { get; set; }
        public int? STC_FGIN { get; set; }
        public int? STC_Forward { get; set; }
        public int? Balance { get; set; }
        [StringLength(10)]
        public string Building { get; set; }
    }
}
