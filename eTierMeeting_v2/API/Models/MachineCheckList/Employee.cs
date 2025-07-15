using System.ComponentModel.DataAnnotations;

namespace Machine_API.Models.MachineCheckList
{
    public class Employee
    {
        public int ID { get; set; }

        [StringLength(200)]
        public string EmpName { get; set; }

        [StringLength(20)]
        public string EmpNumber { get; set; }

        public bool? Visible { get; set; }

        [StringLength(20)]
        public string UpdateBy { get; set; }

        public DateTime? UpdateTime { get; set; }
    }
}