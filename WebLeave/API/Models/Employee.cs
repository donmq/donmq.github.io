
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models
{
    [Table("Employee")]
    [Index("EmpNumber", Name = "UQ__Employee__FA28D5FF6465DD6D", IsUnique = true)]
    public partial class Employee
    {
        public Employee()
        {
            HistoryEmps = new HashSet<HistoryEmp>();
            LeaveData = new HashSet<LeaveData>();
            ReportData = new HashSet<ReportData>();
            Users = new HashSet<Users>();
        }

        [Key]
        public int EmpID { get; set; }
        [StringLength(200)]
        public string EmpName { get; set; }
        [StringLength(20)]
        [Unicode(false)]
        public string EmpNumber { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DateIn { get; set; }
        public int? PositionID { get; set; }
        [StringLength(400)]
        public string Descript { get; set; }
        public int? GBID { get; set; }
        public bool? Visible { get; set; }
        public int? PartID { get; set; }
        public bool? IsSun { get; set; }

        [ForeignKey("GBID")]
        [InverseProperty("Employees")]
        public virtual GroupBase GB { get; set; }
        [ForeignKey("PartID")]
        [InverseProperty("Employees")]
        public virtual Part Part { get; set; }
        [ForeignKey("PositionID")]
        [InverseProperty("Employees")]
        public virtual Position Position { get; set; }
        [InverseProperty("Emp")]
        public virtual ICollection<HistoryEmp> HistoryEmps { get; set; }
        [InverseProperty("Emp")]
        public virtual ICollection<LeaveData> LeaveData { get; set; }
        [InverseProperty("Emp")]
        public virtual ICollection<ReportData> ReportData { get; set; }
        [InverseProperty("Emp")]
        public virtual ICollection<Users> Users { get; set; }
    }
}