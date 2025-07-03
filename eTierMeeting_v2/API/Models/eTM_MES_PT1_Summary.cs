using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eTierV2_API.Models
{
    public class eTM_MES_PT1_Summary
    {
        [Key]
        [StringLength(5)]
        public string Dept_ID { get; set; }
        [Key]
        [Column(TypeName = "date")]
        public DateTime Data_Date { get; set; }
        [Key]
        [StringLength(4)]
        public string Reason_Code { get; set; }
        [Key]
        [StringLength(2)]
        public string In_Ex { get; set; }
        [Key]
        [StringLength(4)]
        public string Action_Code { get; set; }
        public int Impact_Qty { get; set; }
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