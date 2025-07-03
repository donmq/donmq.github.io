using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eTierV2_API.Models
{
    public class HP_Production_Line_ie21
    {
        [Required]
        [StringLength(10)]
        public string Factory_ID { get; set; }
        [Required]
        [StringLength(3)]
        public string Dept_ID { get; set; }
        [StringLength(1)]
        public string Line_No { get; set; }
        [Column(TypeName = "decimal(2, 0)")]
        public decimal? Base_Hr { get; set; }
        [StringLength(10)]
        public string Plant { get; set; }
        [StringLength(6)]
        public string HP_User { get; set; }
        [Column(TypeName = "date")]
        public DateTime? HP_Date { get; set; }
        [StringLength(50)]
        public string Update_By { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? Update_Time { get; set; }
    }
}