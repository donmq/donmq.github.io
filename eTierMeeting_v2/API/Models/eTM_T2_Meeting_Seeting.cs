using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;

namespace eTierV2_API.Models
{
    public class eTM_T2_Meeting_Seeting
    {
        [Key]
        [Column(TypeName = "date")]
        public DateTime Meeting_Date { get; set; }
        
        [Key]
        [StringLength(50)]
        public string TU_ID { get; set; }
        public TimeSpan Start_Time { get; set; }
        public TimeSpan End_Time { get; set; }

        [Required]
        [StringLength(10)]
        public string Update_By { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime Update_At { get; set; }
    }
}