
using System.ComponentModel.DataAnnotations;

namespace Machine_API.Models.MachineCheckList
{
    public class UserRoles
    {
        [Key]
        public int Roles { get; set; }
        [Key]
        [StringLength(20)]
        public string EmpNumber { get; set; }
        [StringLength(20)]
        public string UpdateBy { get; set; }
        public DateTime? UpdateTime { get; set; }
        [Required]
        [StringLength(20)]
        public string CreateBy { get; set; }
        public DateTime CreateTime { get; set; }
        public bool? Active { get; set; }
    }
}