
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models
{
    [Table("Category")]
    public partial class Category
    {
        public Category()
        {
            CatLangs = new HashSet<CatLang>();
            LeaveData = new HashSet<LeaveData>();
        }

        [Key]
        public int CateID { get; set; }
        [StringLength(100)]
        public string CateName { get; set; }
        [StringLength(10)]
        [Unicode(false)]
        public string CateSym { get; set; }
        public bool? exhibit { get; set; }
        public bool? Visible { get; set; }
        public int? orderby { get; set; }

        [InverseProperty("Cate")]
        public virtual ICollection<CatLang> CatLangs { get; set; }
        [InverseProperty("Cate")]
        public virtual ICollection<LeaveData> LeaveData { get; set; }
    }
}