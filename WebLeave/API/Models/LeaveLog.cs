using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models
{
    [Table("LeaveLog")]
    public partial class LeaveLog
    {
        [Key]
        public int LogID { get; set; }
        [Required]
        [StringLength(50)]
        public string LeaveType { get; set; }
        [Required]
        [StringLength(100)]
        public string CateName { get; set; }
        [Required]
        [StringLength(20)]
        [Unicode(false)]
        public string EmpNumber { get; set; }
        [Required]
        [StringLength(200)]
        public string EmpName { get; set; }
        [Required]
        [StringLength(20)]
        public string AddedByEmpNumber { get; set; }
        [Required]
        [StringLength(200)]
        public string AddedByEmpName { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime RequestDate { get; set; }
        public int LeaveID { get; set; }
        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string LoggedByIP { get; set; }
    }
}
