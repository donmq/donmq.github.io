using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eTierV2_API.Models
{
    public class eTM_MES_MO_Record
    {
        [Key]
        [Column(TypeName = "date")]
        public DateTime Data_Date { get; set; }
        [Required]
        [StringLength(4)]
        public string Dept_ID { get; set; }
        [Key]
        [StringLength(15)]
        public string MO_No { get; set; }
        [Key]
        [StringLength(3)]
        public string MO_Seq { get; set; }
        [Column(TypeName = "date")]
        public DateTime? Confirmed_Date { get; set; }
        [Column(TypeName = "date")]
        public DateTime? Plan_Finish_Date { get; set; }
        [Column(TypeName = "date")]
        public DateTime? Plan_Ship_Date { get; set; }
        [StringLength(15)]
        public string Style_No { get; set; }
        [StringLength(40)]
        public string Style_Name { get; set; }
        [StringLength(10)]
        public string Color_No { get; set; }
        public int Plan_Qty { get; set; }
        public int Output_Qty { get; set; }
        public int FGIN_Qty { get; set; }
        [StringLength(40)]
        public string Nation { get; set; }
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