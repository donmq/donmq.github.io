using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models
{
    public partial class LunchBreak
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(20)]
        public string Key { get; set; }
        [Precision(0)]
        public TimeSpan WorkTimeStart { get; set; }
        [Precision(0)]
        public TimeSpan WorkTimeEnd { get; set; }
        [Precision(0)]
        public TimeSpan LunchTimeStart { get; set; }
        [Precision(0)]
        public TimeSpan LunchTimeEnd { get; set; }
        [StringLength(250)]
        public string Value_vi { get; set; }
        [StringLength(250)]
        public string Value_en { get; set; }
        [StringLength(250)]
        public string Value_zh { get; set; }
        public int? Seq { get; set; }
        public bool? Visible { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedTime { get; set; }
        public int? UpdatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedTime { get; set; }
    }
}