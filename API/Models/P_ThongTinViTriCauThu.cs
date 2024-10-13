using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

[Table("P_ThongTinViTriCauThu")]
public partial class P_ThongTinViTriCauThu
{
    [Key]
    [Column("ID")]
    public int ID { get; set; }

    [Column("ThongTinID")]
    public int? ThongTinID { get; set; }

    [Column("ViTriID")]
    public int? ViTriID { get; set; }
}
