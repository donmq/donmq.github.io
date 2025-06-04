
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models
{
    [Table("Area")]
    public partial class Area
    {
        public Area()
        {
            AreaLangs = new HashSet<AreaLang>();
            Buildings = new HashSet<Building>();
            CountAreas = new HashSet<CountArea>();
            Departments = new HashSet<Department>();
        }

        [Key]
        public int AreaID { get; set; }
        [StringLength(150)]
        public string AreaName { get; set; }
        [StringLength(10)]
        [Unicode(false)]
        public string AreaSym { get; set; }
        [StringLength(16)]
        [Unicode(false)]
        public string AreaCode { get; set; }
        public int? CompanyID { get; set; }
        public int? Number { get; set; }
        public bool? Visible { get; set; }

        [ForeignKey("CompanyID")]
        [InverseProperty("Areas")]
        public virtual Company Company { get; set; }
        [InverseProperty("Area")]
        public virtual ICollection<AreaLang> AreaLangs { get; set; }
        [InverseProperty("Area")]
        public virtual ICollection<Building> Buildings { get; set; }
        [InverseProperty("Count_Area")]
        public virtual ICollection<CountArea> CountAreas { get; set; }
        [InverseProperty("Area")]
        public virtual ICollection<Department> Departments { get; set; }
    }
}