using System.ComponentModel.DataAnnotations;

namespace Machine_API.Models.MachineCheckList
{
    public class Building
    {
        [Key]        
        public int BuildingID { get; set; }
        [StringLength(50)]
        public string BuildingCode { get; set; }
        [StringLength(50)]
        public string BuildingName { get; set; }
        public bool? Visible { get; set; }
    }
}