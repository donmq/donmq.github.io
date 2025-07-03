using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eTierV2_API.Models
{
    public partial class eTM_HP_Efficiency_Data_External
    {
        [Key]
        public int ID { get; set; }

        public string Date_Range_Type { get; set; }

        public string Date_Range_Label { get; set; }

        [Column(TypeName = "decimal(10, 3)")]
        public decimal? Sequence { get; set; }

        public string Performance_Type { get; set; }

        [Column(TypeName = "decimal(10, 3)")]
        public decimal? SHC { get; set; }

        [Column(TypeName = "decimal(10, 3)")]
        public decimal? CB { get; set; }

        [Column(TypeName = "decimal(10, 3)")]
        public decimal? TSH { get; set; }
    }
}
