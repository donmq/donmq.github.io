
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models
{
    public partial class Roles
    {
        public Roles()
        {
            Roles_User = new HashSet<Roles_User>();
        }

        [Key]
        public int RoleID { get; set; }
        [StringLength(100)]
        public string RoleName { get; set; }
        [StringLength(10)]
        [Unicode(false)]
        public string RoleSym { get; set; }
        public int? Ranked { get; set; }
        public int? GroupIN { get; set; }

        [InverseProperty("Role")]
        public virtual ICollection<Roles_User> Roles_User { get; set; }
    }
}