using System.ComponentModel.DataAnnotations;

namespace Machine_API.Models.MachineCheckList
{
    public class Cells
    {
        public int CellID { get; set; }

        [StringLength(50)]
        public string CellCode { get; set; }

        [StringLength(50)]
        public string CellName { get; set; }

        public int? PDCID { get; set; }

        public int BuildingID { get; set; }

        public bool? Visible { get; set; }

        [StringLength(20)]
        public string UpdateBy { get; set; }

        public DateTime? UpdateTime { get; set; }
    }
}