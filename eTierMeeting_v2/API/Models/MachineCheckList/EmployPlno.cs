using System.ComponentModel.DataAnnotations;

namespace Machine_API.Models.MachineCheckList
{
    public class EmployPlno
    {
        public int ID { get; set; }

        [StringLength(20)]
        public string Plno { get; set; }

        [StringLength(20)]
        public string EmpNumber { get; set; }

        [StringLength(20)]
        public string UpdateBy { get; set; }

        public DateTime? UpdateTime { get; set; }
        
    }
}