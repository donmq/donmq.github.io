// Generated at 3/4/2024, 3:22:31 PM
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace eTierV2_API.Models
{
    [Table("VW_T2_Meeting_Log")]
    public partial class VW_T2_Meeting_Log
    {
        [Required]
        public DateTime Meeting_Date { get; set; }
        [Required]
        public string Level { get; set; }
        [StringLength(50)]
        public string PDC { get; set; }
        [StringLength(50)]
        public string Building { get; set; }
        [Required]
        [StringLength(50)]
        public string TU_ID { get; set; }
        public decimal? Plan_Hour { get; set; }
        public DateTime? Meeting_Start_Time { get; set; }
        public DateTime? Meeting_End_Time { get; set; }
        public int? Duration_Sec { get; set; }
        [Required]
        public int Perform { get; set; }
        [Required]
        public int Effective { get; set; }
        public DateTime? Safety_Start_Time { get; set; }
        public DateTime? Safety_End_Time { get; set; }
        public int? Safety_Duration { get; set; }
        public DateTime? Quality_Start_Time { get; set; }
        public DateTime? Quality_End_Time { get; set; }
        public int? Quality_Duration { get; set; }
        public DateTime? Efficiency_Start_Time { get; set; }
        public DateTime? Efficiency_End_Time { get; set; }
        public int? Efficiency_Duration { get; set; }
        public DateTime? Kaizen_Start_Time { get; set; }
        public DateTime? Kaizen_End_Time { get; set; }
        public int? Kaizen_Duration { get; set; }
    }
}