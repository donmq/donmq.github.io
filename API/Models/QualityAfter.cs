// Generated at 11/21/2024, 11:29:24 AM
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace API.Models
{
    [Table("QualityAfter")]
    public partial class QualityAfter
    {
        [Key]
        [Required]
        public int InforID { get; set; }
        [Key]
        [Required]
        public int ExerciseID { get; set; }
        public int? Average { get; set; }
        public int? CanPha { get; set; }
        public int? KemNguoi { get; set; }
        public int? ChayCho { get; set; }
        public int? DanhDau { get; set; }
        public int? DungCam { get; set; }
        public int? ChuyenBong { get; set; }
        public int? ReBong { get; set; }
        public int? TatCanh { get; set; }
        public int? SutManh { get; set; }
        public int? DutDiem { get; set; }
        public int? TheLuc { get; set; }
        public int? SucManh { get; set; }
        public int? XongXao { get; set; }
        public int? TocDo { get; set; }
        public int? SangTao { get; set; }
    }
}