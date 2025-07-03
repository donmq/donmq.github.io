using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace eTierV2_API.Models
{
    public partial class eTM_Dept_Score_Data
    {
        [Key]
        [StringLength(4)]
        public string Dept_ID { get; set; }
        [Key]
        [Column(TypeName = "date")]
        public DateTime Data_Date { get; set; }
        [Column(TypeName = "decimal(8, 1)")]
        public decimal? RFT { get; set; }
        [Column(TypeName = "decimal(8, 1)")]
        public decimal? BA { get; set; }
        [Column(TypeName = "decimal(8, 2)")]
        public decimal? Dept_Output_Target { get; set; }
        [Column(TypeName = "decimal(8, 2)")]
        public decimal? Output_FGIN { get; set; }
        [Column(TypeName = "decimal(8, 2)")]
        public decimal? Defect_Qty { get; set; }
        [Column(TypeName = "decimal(8, 2)")]
        public decimal? Impact_Qty_4M { get; set; }
        [Column(TypeName = "decimal(8, 2)")]
        public decimal? Plan_Working_Hrs { get; set; }
        [Column(TypeName = "decimal(8, 2)")]
        public decimal? Actual_Working_Hrs { get; set; }
        [Required]
        [StringLength(10)]
        public string Insert_By { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime Insert_Time { get; set; }
        [StringLength(10)]
        public string Update_By { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? Update_Time { get; set; }
    }
}
