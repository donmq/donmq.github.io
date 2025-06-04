
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models
{
    [Table("CatLang")]
    public partial class CatLang
    {
        [Key]
        public int CateLangID { get; set; }
        [StringLength(100)]
        public string CateName { get; set; }
        [StringLength(6)]
        [Unicode(false)]
        public string LanguageID { get; set; }
        public int? CateID { get; set; }
        public int? Position { get; set; }

        [ForeignKey("CateID")]
        [InverseProperty("CatLangs")]
        public virtual Category Cate { get; set; }
    }
}