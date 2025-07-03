using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eTierV2_API.Models
{
    public class eTM_Team_Unit
    {
        [Key]
        [StringLength(10)]
        public string TU_ID { get; set; }
        [Required]
        [StringLength(50)]
        public string TU_Name { get; set; }
        [Required]
        [StringLength(20)]
        public string Center_Level { get; set; }
        [Required]
        [StringLength(3)]
        public string Tier_Level { get; set; }
        [Required]
        [StringLength(8)]
        public string TU_Code { get; set; }
        [StringLength(50)]
        public string Class1_Level { get; set; }
        [StringLength(50)]
        public string Class2_Level { get; set; }
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
    public class eTM_Team_UnitIndexOC
    {
        public int Id { get; set; }
        public int Level { get; set; }
         public int? ParentID { get; set; }
        public string TU_ID { get; set; }
        public string TU_Name { get; set; }
        public string Center_Level { get; set; }
        public string Tier_Level { get; set; }
        public string TU_Code { get; set; }
        public string Class1_Level { get; set; }
        public string Class2_Level { get; set; }
        public int? SortSeq { get; set; }      // Sort sequence by Parent Id
        public int? LineNum { get; set; }      // The first LineNum of Child level
        public int? RowCount { get; set; }
        public string Building { get; set; }
        public bool IsActive { get; set; }
    }
}