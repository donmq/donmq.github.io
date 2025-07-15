// Generated at 11/28/2024, 2:39:46 PM
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Machine_API.Models.MachineCheckList
{
    [Table("Machine_Safe_Checklist")]
    public partial class Machine_Safe_Checklist
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [StringLength(50)]
        public string ChecklistName_EN { get; set; }
        [StringLength(50)]
        public string ChecklistName_Local { get; set; }
        [StringLength(50)]
        public string ChecklistName_CN { get; set; }
        public DateTime? CreateTime { get; set; }
        [StringLength(255)]
        public string CreateBy { get; set; }
    }
}