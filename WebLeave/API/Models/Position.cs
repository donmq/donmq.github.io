
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models
{
    [Table("Position")]
    public partial class Position
    {
        public Position()
        {
            Employees = new HashSet<Employee>();
            PosLangs = new HashSet<PosLang>();
        }

        [Key]
        public int PositionID { get; set; }
        [StringLength(100)]
        public string PositionName { get; set; }
        [StringLength(12)]
        [Unicode(false)]
        public string PositionSym { get; set; }

        [InverseProperty("Position")]
        public virtual ICollection<Employee> Employees { get; set; }
        [InverseProperty("Position")]
        public virtual ICollection<PosLang> PosLangs { get; set; }
    }
}