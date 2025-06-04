
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace API.Models
{
    [Table("Roles_User")]
    public partial class Roles_User
    {
        [Key]
        public int RolesUserID { get; set; }
        public int? RoleID { get; set; }
        public int? UserID { get; set; }
        public int? GroupIN { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? Updated { get; set; }
        [StringLength(250)]
        public string Updated_By { get; set; }

        [ForeignKey(nameof(RoleID))]
        [InverseProperty(nameof(Roles.Roles_User))]
        public virtual Roles Role { get; set; }
        [ForeignKey(nameof(UserID))]
        [InverseProperty(nameof(Users.Roles_User))]
        public virtual Users User { get; set; }
    }
}