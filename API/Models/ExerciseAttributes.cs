// Generated at 12/7/2024, 4:02:57 PM
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace API.Models
{
    [Table("ExerciseAttributes")]
    public partial class ExerciseAttributes
    {
        [Key]
        [Required]
        public int ExerciseID { get; set; }
        [Key]
        [Required]
        public int AttributeID { get; set; }
    }
}