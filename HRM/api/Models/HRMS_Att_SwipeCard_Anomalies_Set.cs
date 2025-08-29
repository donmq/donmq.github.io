using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models
{
    [PrimaryKey("Factory", "Seq", "Work_Shift_Type")]
    public partial class HRMS_Att_SwipeCard_Anomalies_Set
    {
        [Key]
        [Required]
        public string Factory { get; set; }
        [Key]
        [Required]
        [Column("Work_Shift_Type")]
        public string Work_Shift_Type { get; set; }
        public string Kind { get; set; }
        [Key]
        [Required]
        public short Seq { get; set; }
        [Column("Clock_In")]
        public string Clock_In { get; set; }
        [Column("Clock_Out_Start")]
        public string Clock_Out_Start { get; set; }
        [Column("Clock_Out_End")]
        public string Clock_Out_End { get; set; }
        [Required]
        [Column("Update_By")]
        public string Update_By { get; set; }
        [Required]
        [Column("Update_Time",TypeName = "datetime")]
        public DateTime Update_Time { get; set; }
    }
}