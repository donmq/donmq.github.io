// Generated at 12/7/2024, 4:02:59 PM
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