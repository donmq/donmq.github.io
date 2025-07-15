using System.ComponentModel.DataAnnotations;

namespace Machine_API.Models.MachineCheckList
{
    public class DateInventory
    {
        public int Id { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        [StringLength(255)]
        public string Content { get; set; }

        [StringLength(50)]
        public string EmpName { get; set; }

        public DateTime? CreateTime { get; set; }

        [StringLength(255)]
        public string CreateBy{ get; set; }
        
    }
}