using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace eTierV2_API.Models
{
    [Table("VW_Prod_T1_CTB_Delivery")]
    public partial class VW_Prod_T1_CTB_Delivery
    {
        [StringLength(200)]
        public string ASL_Team { get; set; }
        [StringLength(200)]
        public string STI_Team { get; set; }
        public DateTime? ETC { get; set; }
        [StringLength(200)]
        public string PO_Batch { get; set; }
        [StringLength(200)]
        public string Model_Name { get; set; }
        [StringLength(200)]
        public string Article { get; set; }
        public decimal? Planned_Qty { get; set; }
        public int? Balance { get; set; }
        [StringLength(200)]
        public string Country { get; set; }
    }
}