
using System.ComponentModel.DataAnnotations;

namespace Machine_API.Models.MachineCheckList
{
    public class PreliminaryPlno
    {
        [Required]
        [StringLength(20)]
        public string Plno { get; set; }
        [Required]
        [StringLength(20)]
        public string EmpNumber { get; set; }
        [StringLength(20)]
        public string UpdateBy { get; set; }
        public DateTime? UpdateTime { get; set; }
        [Required]
        [StringLength(20)]
        public int? CellID { get; set; }
        public int BuildingID { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateTime { get; set; }
        public bool Active { get; set; }
        public string CellCode { get; set; }
        public bool? Is_Manager{get; set;}
        public bool? Is_Preliminary{get; set;}

    }
}