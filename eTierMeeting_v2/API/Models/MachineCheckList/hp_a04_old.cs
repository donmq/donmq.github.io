using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Machine_API.Models.MachineCheckList
{
    public class hp_a04_old
    {
        [Key]
        [StringLength(20)]
        public string AssnoID { get; set; }
        [StringLength(100)]
        public string Spec { get; set; }
        [StringLength(20)]
        public string Plno { get; set; }
        [StringLength(10)]
        public string State { get; set; }
        [StringLength(100)]
        public string MachineName_EN { get; set; }
        [StringLength(100)]
        public string MachineName_Local { get; set; }
        [StringLength(100)]
        public string MachineName_CN { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? Trdate { get; set; }
        public bool? Visible { get; set; }
        [StringLength(100)]
        public string Supplier { get; set; }
        [Key]
        [StringLength(1)]
        public string OwnerFty { get; set; }
        [StringLength(3)]
        public string Askid { get; set; }
    }
}