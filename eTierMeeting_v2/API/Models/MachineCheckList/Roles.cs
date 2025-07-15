
using System.ComponentModel.DataAnnotations;

namespace Machine_API.Models.MachineCheckList
{
    public class Roles
    {
        [Key]
        public int ID { get; set; }
        [Required]
        [StringLength(50)]
        public string RoleName { get; set; }
        [Required]
        [StringLength(20)]
        public string UpdateBy { get; set; }
        public DateTime? UpdateTime { get; set; }
        public int? RoleSequence { get; set; }
    }
}