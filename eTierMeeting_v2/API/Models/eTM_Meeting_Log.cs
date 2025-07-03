using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eTierV2_API.Models
{
    public class eTM_Meeting_Log
    {
        [Key]
        public Guid Record_ID { get; set; }
        [Required]
        [StringLength(10)]
        public string TU_ID { get; set; }
        [Column(TypeName = "date")]
        public DateTime Data_Date { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime Meeting_Start_Time { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? Meeting_End_Time { get; set; }
        [Required]
        [StringLength(20)]
        public string Record_Status { get; set; }
        public int? Duration_Sec { get; set; }
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