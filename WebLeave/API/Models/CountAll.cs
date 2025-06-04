
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models
{
    [Table("CountAll")]
    public partial class CountAll
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
        public int? Count_ComID { get; set; }
        [Column(TypeName = "date")]
        public DateTime? Count_Date { get; set; }

        [ForeignKey("Count_ComID")]
        [InverseProperty("CountAlls")]
        public virtual Company Count_Com { get; set; }
    }
}