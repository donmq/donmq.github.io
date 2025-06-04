
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models
{
    public partial class LeaveData
    {
        [Key]
        public int LeaveID { get; set; }
        [StringLength(30)]
        [Unicode(false)]
        public string LeaveArchive { get; set; }
        public int? EmpID { get; set; }
        public int? CateID { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? Time_Start { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? Time_End { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DateLeave { get; set; }
        public double? LeaveDay { get; set; }
        public int? Approved { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? Time_Applied { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string TimeLine { get; set; }
        public string Comment { get; set; }
        public bool? LeavePlus { get; set; }
        public bool? LeaveArrange { get; set; }
        public int? UserID { get; set; }
        public int? ApprovedBy { get; set; }
        public int? EditRequest { get; set; }
        public bool? Status_Line { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? Created { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? Updated { get; set; }
        public bool? Status_Lock { get; set; }
        [StringLength(255)]
        public string MailContent_Lock { get; set; }
        [StringLength(500)]
        public string CommentArchive { get; set; }
        [StringLength(500)]
        public string ReasonAdjust { get; set; }

        [ForeignKey("ApprovedBy")]
        [InverseProperty("LeaveDataApprovedByNavigations")]
        public virtual Users ApprovedByNavigation { get; set; }
        [ForeignKey("CateID")]
        [InverseProperty("LeaveData")]
        public virtual Category Cate { get; set; }
        [ForeignKey("EmpID")]
        [InverseProperty("LeaveData")]
        public virtual Employee Emp { get; set; }
        [ForeignKey("UserID")]
        [InverseProperty("LeaveDataUsers")]
        public virtual Users User { get; set; }
    }
}