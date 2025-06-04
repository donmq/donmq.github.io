
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models
{
    [Table("Company")]
    public partial class Company
    {
        public Company()
        {
            Areas = new HashSet<Area>();
            CountAlls = new HashSet<CountAll>();
        }

        [Key]
        public int CompanyID { get; set; }
        [StringLength(200)]
        public string CompanyName { get; set; }
        [StringLength(400)]
        public string CompanyInfo { get; set; }
        [StringLength(10)]
        [Unicode(false)]
        public string CompanySym { get; set; }
        public int? Number { get; set; }
        public bool? Visible { get; set; }

        [InverseProperty("Company")]
        public virtual ICollection<Area> Areas { get; set; }
        [InverseProperty("Count_Com")]
        public virtual ICollection<CountAll> CountAlls { get; set; }
    }
}
