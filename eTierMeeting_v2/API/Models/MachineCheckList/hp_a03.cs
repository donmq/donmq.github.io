using System.ComponentModel.DataAnnotations;

namespace Machine_API.Models.MachineCheckList
{
    public class hp_a03
    {
        [Key]
        [StringLength(50)]
        public string Askid { get; set; }

        [StringLength(50)]
        public string Kinen_Local { get; set; }

        [StringLength(50)]
        public string Kinen_EN { get; set; }

        [StringLength(50)]
        public string Kinen_CN { get; set; }

        public bool? Visible { get; set; }
    }
}