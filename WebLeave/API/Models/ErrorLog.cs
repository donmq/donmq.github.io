
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("ErrorLog")]
    public partial class ErrorLog
    {
        [Key]
        public int ID { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DateLog { get; set; }
        [Column(TypeName = "ntext")]
        public string Content { get; set; }
    }
}