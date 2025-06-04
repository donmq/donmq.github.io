
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    public partial class ReportData
    {
        [Key]
        public long id { get; set; }
        public int? EmpID { get; set; }
        public int? LeaveID { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? LeaveDate { get; set; }
        public int? DayOfWeek { get; set; }
        public int? StatusLine { get; set; }
        public int? PartTotalEmp { get; set; }
        public int? DeptTotalEmp { get; set; }
        public int? BuildingTotalEmp { get; set; }
        public int? AreaTotalEmp { get; set; }
        public int? PartTotalEmpByEmp { get; set; }
        public int? DeptTotalEmpByEmp { get; set; }
        public int? BuildingTotalEmpByEmp { get; set; }
        public int? AreaTotalEmpByEmp { get; set; }
        public int? CompTotalEmp { get; set; }
        public int? MPPoolOut { get; set; }
        public int? MPPoolIn { get; set; }
        public int? Total { get; set; }

        [ForeignKey("EmpID")]
        [InverseProperty("ReportData")]
        public virtual Employee Emp { get; set; }
     
    }
}