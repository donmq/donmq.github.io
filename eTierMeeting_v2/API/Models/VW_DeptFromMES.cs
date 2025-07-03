using System.ComponentModel.DataAnnotations;

namespace eTierV2_API.Models
{
    public partial class VW_DeptFromMES
    {
        [Required]
        [StringLength(4)]
        public string Dept_ID { get; set; }
        [Required]
        [StringLength(3)]
        public string Line_ID { get; set; }
        [StringLength(3)]
        public string Line_ID_2 { get; set; }
        [StringLength(10)]
        public string Line_Sname { get; set; }
        [StringLength(10)]
        public string Building { get; set; }
        [StringLength(3)]
        public string PS_ID { get; set; }
    }
}