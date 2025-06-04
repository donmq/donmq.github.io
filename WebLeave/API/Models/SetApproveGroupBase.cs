
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace API.Models
{
    [Table("SetApproveGroupBase")]
    public partial class SetApproveGroupBase
    {
        [Key]
        public int SAGBID { get; set; }
        public int? GBID { get; set; }
        public int? UserID { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? Created { get; set; }

        [ForeignKey(nameof(GBID))]
        [InverseProperty(nameof(GroupBase.SetApproveGroupBase))]
        public virtual GroupBase GB { get; set; }
        [ForeignKey(nameof(UserID))]
        [InverseProperty(nameof(Users.SetApproveGroupBase))]
        public virtual Users User { get; set; }
    }
}