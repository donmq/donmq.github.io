
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models
{
    [Table("CountArea")]
    public partial class CountArea
    {
        [Key]
        public int Count_ID { get; set; }
        public int? Count_AllEmp { get; set; }
        public int? Count_Apply { get; set; }
        public int? Count_Approve { get; set; }
        public int? Count_Actual { get; set; }
        [StringLength(30)]
        [Unicode(false)]
        public string Count_Time { get; set; }
        public int? Count_AreaID { get; set; }
        [Column(TypeName = "date")]
        public DateTime? Count_Date { get; set; }

        [ForeignKey("Count_AreaID")]
        [InverseProperty("CountAreas")]
        public virtual Area Count_Area { get; set; }
    }
}