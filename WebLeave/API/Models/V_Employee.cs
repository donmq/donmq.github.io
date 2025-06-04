using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace API.Models
{
    [Keyless]
    public partial class V_Employee
    {
        [StringLength(20)]
        [Unicode(false)]
        public string EmpNumber { get; set; }
        [StringLength(200)]
        public string EmpName { get; set; }
        [StringLength(12)]
        [Unicode(false)]
        public string PositionSym { get; set; }
        [StringLength(100)]
        public string PositionName { get; set; }
    }
}
