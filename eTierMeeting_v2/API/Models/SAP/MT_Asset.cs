using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Machine_API.Models.SAP
{
    public partial class MT_Asset
    {
        [Key]
        [StringLength(20)]
        public string SID { get; set; }
        [Key]
        [StringLength(4)]
        public string Owner_Fty { get; set; }
        [Key]
        [StringLength(12)]
        public string Main_Asset_Number { get; set; }
        [Key]
        [StringLength(4)]
        public string Assno_ID { get; set; }
        [Required]
        [StringLength(8)]
        public string Askid { get; set; }
        [Required]
        [StringLength(50)]
        public string Machine_Name_CN { get; set; }
        [Required]
        [StringLength(50)]
        public string Machine_Name_EN { get; set; }
        [Required]
        [StringLength(50)]
        public string Machine_Name_Local { get; set; }
        [Required]
        [StringLength(18)]
        public string Spec { get; set; }
        [Required]
        [StringLength(25)]
        public string HP_Assno_ID { get; set; }
        [Column(TypeName = "decimal(13, 0)")]
        public decimal? Quantity { get; set; }
        [Required]
        [StringLength(10)]
        public string Dept_ID { get; set; }
        [Required]
        [StringLength(10)]
        public string Plno { get; set; }
        [Required]
        [StringLength(10)]
        public string Supplier_ID { get; set; }
        [Required]
        [StringLength(30)]
        public string Supplier_Name { get; set; }
        [StringLength(8)]
        public string Trdate { get; set; }
        [StringLength(8)]
        public string Deactivation_Date { get; set; }
        [StringLength(50)]
        public string Update_By { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? Update_Time { get; set; }
        [StringLength(1)]
        public string HP_Flag { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? HP_Time { get; set; }
        [StringLength(1)]
        public string Biz_Flag { get; set; }
        [StringLength(1)]
        public string Biz_Tflag { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? Biz_P_Time { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? Biz_Y_Time { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? Biz_Time { get; set; }
    }
}