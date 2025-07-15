
using System.ComponentModel.DataAnnotations;

namespace Machine_API.Models.MachineCheckList
{
    public class HistoryInventory
    {
        public int HistoryInventoryID { get; set; }

        public int? InventoryType { get; set; }

        [StringLength(20)]
        public string PlnoID { get; set; }

        public int? CountComplete { get; set; }

        public int? CountWrongPosition { get; set; }

        public int? CountNotScan { get; set; }

        [StringLength(20)]
        public string CreateBy { get; set; }

        public DateTime? StartTimeInventory { get; set; }

        public DateTime? EndTimeInventory { get; set; }

        public DateTime? CreateTime { get; set; }

    }
}