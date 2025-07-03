// Generated at 5/21/2025, 7:50:22 AM
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace eTierV2_API.Models
{
    [Table("eTM_HP_Efficiency_Data_Subcon")]
    public partial class eTM_HP_Efficiency_Data_Subcon
    {
        [Key]
        [Required]
        public DateTime Data_Date { get; set; }
        [Key]
        [Required]
        public string Factory_ID { get; set; }
        [Key]
        [Required]
        public string Operation { get; set; }
        [Required]
        public decimal Standard_Whrs { get; set; }
        [Required]
        public decimal Order_Rate { get; set; }
        [Required]
        public decimal Subcon_Whrs { get; set; }
        public DateTime? Update_Time { get; set; }
    }
}