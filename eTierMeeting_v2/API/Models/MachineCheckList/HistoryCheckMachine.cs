using System.ComponentModel.DataAnnotations;

namespace Machine_API.Models.MachineCheckList
{
    public class HistoryCheckMachine
    {
        public int HistoryCheckMachineID { get; set; }

        public int? TotalMachine { get; set; }

        public int? TotalScans { get; set; }
        public int? TotalNotScan { get; set; }

        public int? TotalExist { get; set; }

        public int? TotalNotExist { get; set; }

        [StringLength(20)]
        public string CreateBy { get; set; }

        public DateTime? CreateTime { get; set; }
        
    }
}