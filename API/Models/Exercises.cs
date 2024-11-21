// Generated at 11/21/2024, 11:29:26 AM
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace API.Models
{
    [Table("Exercises")]
    public partial class Exercises
    {
        [Key]
        [Required]
        public int ID { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        public int? Difficult_Level  { get; set; }
        public string Class { get; set; }
    }
}