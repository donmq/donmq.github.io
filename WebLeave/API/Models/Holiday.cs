
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("Holiday")]
    public partial class Holiday
    {
        [Key]
        public int HolidayID { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? Date { get; set; }
        [StringLength(255)]
        public string Description { get; set; }
        public int? UserID { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreateTime { get; set; }

        [ForeignKey("UserID")]
        [InverseProperty("Holidays")]
        public virtual Users User { get; set; }
    }
}