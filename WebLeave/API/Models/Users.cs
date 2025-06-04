
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models
{
    [Index(nameof(UserName), Name = "UQ__Users__C9F284561DA33234", IsUnique = true)]
    public partial class Users
    {
        public Users()
        {
            CommentArchives = new HashSet<CommentArchive>();
            DatePickerManagers = new HashSet<DatePickerManager>();
            Holidays = new HashSet<Holiday>();
            LeaveDataApprovedByNavigations = new HashSet<LeaveData>();
            LeaveDataUsers = new HashSet<LeaveData>();
            Roles_User = new HashSet<Roles_User>();
            SetApproveGroupBase = new HashSet<SetApproveGroupBase>();
        }

        [Key]
        public int UserID { get; set; }
        [StringLength(100)]
        [Unicode(false)]
        public string UserName { get; set; }
        [StringLength(100)]
        [Unicode(false)]
        public string HashPass { get; set; }
        [StringLength(200)]
        [Unicode(false)]
        public string HashImage { get; set; }
        [StringLength(100)]
        [Unicode(false)]
        public string EmailAddress { get; set; }
        public int? UserRank { get; set; }
        public bool? ISPermitted { get; set; }
        public int? EmpID { get; set; }
        public bool? Visible { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? Updated { get; set; }
        [StringLength(100)]
        public string FullName { get; set; }

        [ForeignKey("EmpID")]
        [InverseProperty("Users")]
        public virtual Employee Emp { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<CommentArchive> CommentArchives { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<DatePickerManager> DatePickerManagers { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<Holiday> Holidays { get; set; }
        [InverseProperty("ApprovedByNavigation")]
        public virtual ICollection<LeaveData> LeaveDataApprovedByNavigations { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<LeaveData> LeaveDataUsers { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<Roles_User> Roles_User { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<SetApproveGroupBase> SetApproveGroupBase { get; set; }
    }
}