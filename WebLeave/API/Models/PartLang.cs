
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models
{
    [Table("PartLang")]
    public partial class PartLang
    {
        [Key]
        public int PartLangID { get; set; }
        [StringLength(100)]
        public string PartName { get; set; }
        [StringLength(6)]
        [Unicode(false)]
        public string LanguageID { get; set; }
        public int? PartID { get; set; }

        [ForeignKey("PartID")]
        [InverseProperty("PartLangs")]
        public virtual Part Part { get; set; }
    }
}