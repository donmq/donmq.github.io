// Generated at 12/7/2024, 4:03:00 PM
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace API.Models
{
    [Table("TypeAttributes")]
    public partial class TypeAttributes
    {
        [Required]
        public int PositionID { get; set; }
        [Required]
        public int AttributeID { get; set; }
        [Required]
        public bool Disable { get; set; }
    }
}