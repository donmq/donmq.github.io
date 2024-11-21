// Generated at 11/21/2024, 11:29:25 AM
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace API.Models
{
    [Table("Information")]
    public partial class Information
    {
        [Key]
        [Required]
        public int ID { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(50)]
        public string YearOld { get; set; }
        public int? Quality { get; set; }
    }
}