using System.ComponentModel.DataAnnotations;

namespace eTierV2_API.Models
{
    public class eTM_HP_Dept_Kind
    {
        [Key]
        [StringLength(1)]
        public string Factory_ID { get; set; }
        [Key]
        public int Data_Year { get; set; }
        [Key]
        public int Data_Month { get; set; }
        [Required]
        [StringLength(1)]
        public string Typ { get; set; }
        [Required]
        [StringLength(1)]
        public string Typ1 { get; set; }
        [Required]
        [StringLength(1)]
        public string Kind { get; set; }
        [Key]
        [StringLength(5)]
        public string Dept_ID { get; set; }
        [Required]
        [StringLength(5)]
        public string Line_ID { get; set; }
        [Required]
        [StringLength(20)]
        public string Line_CName { get; set; }
        [Required]
        [StringLength(5)]
        public string Part { get; set; }
        [StringLength(5)]
        public string Build { get; set; }
        [StringLength(5)]
        public string Build_2 { get; set; }
        [StringLength(5)]
        public string Build_1 { get; set; }
        public bool? G01_Flag { get; set; }
        [Required]
        [StringLength(8)]
        public string YYYYMM { get; set; }
    }
}