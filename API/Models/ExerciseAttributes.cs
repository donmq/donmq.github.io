// Generated at 11/21/2024, 11:29:25 AM
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