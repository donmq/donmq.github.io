// Generated at 11/21/2024, 11:29:25 AM
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace API.Models
{
    [Table("PositionInformation")]
    public partial class PositionInformation
    {
        [Key]
        [Required]
        public int InforID { get; set; }
        [Key]
        [Required]
        public int PositionID { get; set; }
    }
}