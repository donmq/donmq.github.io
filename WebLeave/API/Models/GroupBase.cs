
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models
{
    [Table("GroupBase")]
    public partial class GroupBase
    {
        public GroupBase()
        {
            Employees = new HashSet<Employee>();
            GroupLangs = new HashSet<GroupLang>();
            SetApproveGroupBase = new HashSet<SetApproveGroupBase>();
        }

        [Key]
        public int GBID { get; set; }
        [StringLength(100)]
        public string BaseName { get; set; }
        [StringLength(10)]
        [Unicode(false)]
        public string BaseSym { get; set; }

        [InverseProperty("GB")]
        public virtual ICollection<Employee> Employees { get; set; }
        [InverseProperty("GB")]
        public virtual ICollection<GroupLang> GroupLangs { get; set; }
        [InverseProperty("GB")]
        public virtual ICollection<SetApproveGroupBase> SetApproveGroupBase { get; set; }
    }
}