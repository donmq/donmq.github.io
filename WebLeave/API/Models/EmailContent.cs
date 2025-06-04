
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models
{
    [Table("EmailContent")]
    public partial class EmailContent
    {
        [Key]
        public int ID { get; set; }
        public string Content { get; set; }
        [StringLength(100)]
        [Unicode(false)]
        public string KeyCont { get; set; }
        public int? GroupKey { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? Updated { get; set; }
    }
}