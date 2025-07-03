using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eTierV2_API.Models
{
    public class eTM_MES_Quality_Defect_Data
    {
        [Key]
        [StringLength(20)]
        public string Data_Kind { get; set; }
        [Key]
        [StringLength(4)]
        public string Dept_ID { get; set; }
        [Key]
        [Column(TypeName = "date")]
        public DateTime Data_Date { get; set; }
        [Key]
        [StringLength(20)]
        public string Reason_ID { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal Finding_Qty { get; set; }
        [StringLength(50)]
        public string Image_Path { get; set; }
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