// Generated at 11/21/2024, 11:29:27 AM
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace API.Models
{
    [Table("MainAttributes")]
    public partial class MainAttributes
    {
        [Key]
        [Required]
        public int ID { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        public int? Type { get; set; }
    }
}