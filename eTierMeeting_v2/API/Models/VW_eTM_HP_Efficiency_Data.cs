// Generated at 2/26/2024, 10:18:56 AM
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace eTierV2_API.Models
{
    [Table("VW_eTM_HP_Efficiency_Data")]
    public partial class VW_eTM_HP_Efficiency_Data
    {
        [Required]
        public DateTime Data_Date { get; set; }
        [Required]
        public string Factory_ID { get; set; }
        [Required]
        public string Dept_ID { get; set; }
        public int? Actual_Qty { get; set; }
        public int? Impact_Qty { get; set; }
        public int? Target_Qty { get; set; }
        public decimal? Hour_Base { get; set; }
        public decimal? Hour_Overtime { get; set; }
        public decimal? Hour_IE { get; set; }
        public decimal? Hour_Tot { get; set; }
        public decimal? Hour_Ex { get; set; }
        public decimal? Hour_Learn { get; set; }
        public decimal? Hour_Transfer { get; set; }
        public decimal? Hour_In { get; set; }
        public decimal? Hour_Out { get; set; }
        public decimal? Hour_Tot_All { get; set; }
        public int? Qty_2 { get; set; }
        public decimal? Total_Man { get; set; }
        public decimal? Un_Man { get; set; }
        public decimal? Hour_Tot_2 { get; set; }
        public decimal? Hour_UT005 { get; set; }
        public decimal? Hour_OEM { get; set; }
        public decimal? Hour_Tot_CMT { get; set; }
        [StringLength(10)]
        public string Kind { get; set; }
        public int? IsCTB { get; set; }
    }
}