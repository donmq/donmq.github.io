using System.ComponentModel.DataAnnotations;

namespace Machine_API.Models.MachineCheckList
{
    public class DataHistoryCheckMachine
    {
        public int ID { get; set; }

        [StringLength(22)]
        public string MachineID { get; set; }

        [StringLength(100)]
        public string MachineName { get; set; }

        [StringLength(100)]
        public string Supplier { get; set; }

        [StringLength(120)]
        public string Place { get; set; }

        [StringLength(10)]
        public string State { get; set; }

        public int? StatusCheckMachine { get; set; }

        public int? HistoryCheckMachineID { get; set; }
    }
}