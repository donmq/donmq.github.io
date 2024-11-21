// Generated at 11/21/2024, 11:29:27 AM
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