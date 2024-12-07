// Generated at 12/7/2024, 4:02:58 PM
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace API.Models
{
    [Table("Position")]
    public partial class Position
    {
        [Key]
        [Required]
        public int ID { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
    }
}