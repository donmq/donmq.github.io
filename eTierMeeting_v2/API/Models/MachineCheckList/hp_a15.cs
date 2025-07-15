using System.ComponentModel.DataAnnotations;

namespace Machine_API.Models.MachineCheckList
{
    public class hp_a15
    {
        [Key]
        [StringLength(20)]
        public string Plno { get; set; }

        [StringLength(100)]
        public string Place { get; set; }

        [StringLength(10)]
        public string State { get; set; }
    }
}