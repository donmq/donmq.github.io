
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models
{
    [Table("Building")]
    public partial class Building
    {
        public Building()
        {
            BuildLangs = new HashSet<BuildLang>();
            CountBuildings = new HashSet<CountBuilding>();
            Departments = new HashSet<Department>();
        }

        [Key]
        public int BuildingID { get; set; }
        [StringLength(150)]
        public string BuildingName { get; set; }
        [StringLength(10)]
        [Unicode(false)]
        public string BuildingSym { get; set; }
        [StringLength(16)]
        [Unicode(false)]
        public string BuildingCode { get; set; }
        public int? AreaID { get; set; }
        public int? Number { get; set; }
        public bool? Visible { get; set; }

        [ForeignKey("AreaID")]
        [InverseProperty("Buildings")]
        public virtual Area Area { get; set; }
        [InverseProperty("Building")]
        public virtual ICollection<BuildLang> BuildLangs { get; set; }
        [InverseProperty("Count_Build")]
        public virtual ICollection<CountBuilding> CountBuildings { get; set; }
        [InverseProperty("Building")]
        public virtual ICollection<Department> Departments { get; set; }
    }
}
