// Generated at 12/7/2024, 4:02:58 PM
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace API.Models
{
    [Table("PlayerValue")]
    public partial class PlayerValue
    {
        [Required]
        public int Q { get; set; }
        public decimal? Year18 { get; set; }
        public decimal? Year19 { get; set; }
        public decimal? Year20 { get; set; }
        public decimal? Year21 { get; set; }
    }
}