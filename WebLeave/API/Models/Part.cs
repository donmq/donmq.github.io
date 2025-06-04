
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models
{
    [Table("Part")]
    public partial class Part
    {
        public Part()
        {
            CountParts = new HashSet<CountPart>();
            Employees = new HashSet<Employee>();
            PartLangs = new HashSet<PartLang>();
        }

        [Key]
        public int PartID { get; set; }
        [StringLength(200)]
        public string PartName { get; set; }
        [StringLength(10)]
        [Unicode(false)]
        public string PartSym { get; set; }
        [StringLength(16)]
        [Unicode(false)]
        public string PartCode { get; set; }
        public int? Number { get; set; }
        public int? DeptID { get; set; }
        public bool? Visible { get; set; }

        [ForeignKey("DeptID")]
        [InverseProperty("Parts")]
        public virtual Department Dept { get; set; }
        [InverseProperty("Count_Part")]
        public virtual ICollection<CountPart> CountParts { get; set; }
        [InverseProperty("Part")]
        public virtual ICollection<Employee> Employees { get; set; }
        [InverseProperty("Part")]
        public virtual ICollection<PartLang> PartLangs { get; set; }
    }
}