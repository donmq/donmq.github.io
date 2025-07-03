using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eTierV2_API.Models
{
    public partial class eTM_HP_Efficiency_Data
    {
        [Key]
        [Column(TypeName = "date")]
        public DateTime Data_Date { get; set; }
        [Key]
        [StringLength(4)]
        public string Factory_ID { get; set; }
        [Key]
        [StringLength(4)]
        public string Dept_ID { get; set; }
        public int? Actual_Qty { get; set; }
        public int? Impact_Qty { get; set; }
        public int? Target_Qty { get; set; }
        [Column(TypeName = "decimal(8, 2)")]
        public decimal? Hour_Base { get; set; }
        [Column(TypeName = "decimal(8, 2)")]
        public decimal? Hour_Overtime { get; set; }
        [Column(TypeName = "decimal(8, 2)")]
        public decimal? Hour_IE { get; set; }
        [Column(TypeName = "decimal(8, 2)")]
        public decimal? Hour_Tot { get; set; }
        [Column(TypeName = "decimal(8, 2)")]
        public decimal? Hour_Ex { get; set; }
        [Column(TypeName = "decimal(8, 2)")]
        public decimal? Hour_Learn { get; set; }
        [Column(TypeName = "decimal(8, 2)")]
        public decimal? Hour_Transfer { get; set; }
        [Column(TypeName = "decimal(8, 2)")]
        public decimal? Hour_In { get; set; }
        [Column(TypeName = "decimal(8, 2)")]
        public decimal? Hour_Out { get; set; }
        [Column(TypeName = "decimal(8, 2)")]
        public decimal? Hour_Tot_All { get; set; }
        [Column(TypeName = "decimal(8, 2)")]
        public decimal? Hour_Tot_CMT { get; set; }
        public int? Qty_2 { get; set; }
        [Column(TypeName = "decimal(8, 2)")]
        public decimal? Total_Man { get; set; }
        [Column(TypeName = "decimal(8, 2)")]
        public decimal? Un_Man { get; set; }
        [Column(TypeName = "decimal(8, 2)")]
        public decimal? Hour_Tot_2 { get; set; }
        [Column(TypeName = "decimal(8, 2)")]
        public decimal? Hour_UT005 { get; set; }
        [Column(TypeName = "decimal(8, 2)")]
        public decimal? Hour_OEM { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? Update_Time { get; set; }
    }
}