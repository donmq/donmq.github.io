
using System.ComponentModel.DataAnnotations;

namespace Machine_API.Models.MachineCheckList
{
    public class User
    {
        [Key]
        [Required]
        public int UserID { get; set; }

        [StringLength(20)]
        public string UserName { get; set; }

        [StringLength(100)]
        public string HashPass { get; set; }

        [StringLength(200)]
        public string HashImage { get; set; }

        [StringLength(100)]
        public string EmailAddress { get; set; }

        public bool? Visible { get; set; }

        public DateTime? UpdateDate { get; set; }

        [StringLength(20)]
        public string UpdateBy { get; set; }

        [StringLength(50)]
        public string EmpName { get; set; }
    }
}