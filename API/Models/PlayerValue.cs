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
        // public decimal? 18Year { get; set; }
        // public decimal? 19Year { get; set; }
        // public decimal? 20Year { get; set; }
        // public decimal? 21Year { get; set; }
    }
}