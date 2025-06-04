
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models
{
    [Table("PosLang")]
    public partial class PosLang
    {
        [Key]
        public int PosLangID { get; set; }
        [StringLength(100)]
        public string PositionName { get; set; }
        [StringLength(6)]
        [Unicode(false)]
        public string LanguageID { get; set; }
        public int? PositionID { get; set; }

        [ForeignKey("PositionID")]
        [InverseProperty("PosLangs")]
        public virtual Position Position { get; set; }
    }
}