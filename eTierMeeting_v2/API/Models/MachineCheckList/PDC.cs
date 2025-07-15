using System.ComponentModel.DataAnnotations;

namespace Machine_API.Models.MachineCheckList
{
    public class PDC
    {
        public int PDCID { get; set; }

        [StringLength(50)]
        public string PDCName { get; set; }

        [StringLength(50)]
        public string PDCCode { get; set; }

        public bool? Visible { get; set; }
    }
}