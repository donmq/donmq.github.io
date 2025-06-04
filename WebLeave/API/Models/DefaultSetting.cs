
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models
{
    [Table("DefaultSetting")]
    public partial class DefaultSetting
    {
        [Key]
        public int ID { get; set; }
        [Required]
        [StringLength(100)]
        [Unicode(false)]
        public string KeySett { get; set; }
        public bool? ValueSett { get; set; }
        public int? GroupSett { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? Updated { get; set; }
    }
}