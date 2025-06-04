
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("HistoryEmp")]
    public partial class HistoryEmp
    {
        [Key]
        public int HisrotyID { get; set; }
        public int? EmpID { get; set; }
        public int? YearIn { get; set; }
        public double? TotalDay { get; set; }
        public double? Arrange { get; set; }
        public double? Agent { get; set; }
        public double? CountArran { get; set; }
        public double? CountRestArran { get; set; }
        public double? CountAgent { get; set; }
        public double? CountRestAgent { get; set; }
        public double? CountTotal { get; set; }
        public double? CountLeave { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? Updated { get; set; }

        [ForeignKey("EmpID")]
        [InverseProperty("HistoryEmps")]
        public virtual Employee Emp { get; set; }
    }
}