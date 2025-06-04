
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models
{
    [Table("Department")]
    public partial class Department
    {
        public Department()
        {
            CountDepartments = new HashSet<CountDepartment>();
            DetpLangs = new HashSet<DetpLang>();
            Parts = new HashSet<Part>();
        }

        [Key]
        public int DeptID { get; set; }
        [StringLength(150)]
        public string DeptName { get; set; }
        public int? AreaID { get; set; }
        public int? BuildingID { get; set; }
        [StringLength(16)]
        [Unicode(false)]
        public string DeptCode { get; set; }
        [StringLength(10)]
        [Unicode(false)]
        public string DeptSym { get; set; }
        public int? Number { get; set; }
        public int? Shift_Time { get; set; }
        public bool? Visible { get; set; }

        [ForeignKey("AreaID")]
        [InverseProperty("Departments")]
        public virtual Area Area { get; set; }
        [ForeignKey("BuildingID")]
        [InverseProperty("Departments")]
        public virtual Building Building { get; set; }
        [InverseProperty("Count_Dept")]
        public virtual ICollection<CountDepartment> CountDepartments { get; set; }
        [InverseProperty("Dept")]
        public virtual ICollection<DetpLang> DetpLangs { get; set; }
        [InverseProperty("Dept")]
        public virtual ICollection<Part> Parts { get; set; }
    }
}