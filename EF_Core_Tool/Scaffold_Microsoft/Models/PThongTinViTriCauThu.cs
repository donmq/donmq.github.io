using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

[Table("P_ThongTinViTriCauThu")]
public partial class PThongTinViTriCauThu
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("ThongTinID")]
    public int? ThongTinId { get; set; }

    [Column("ViTriID")]
    public int? ViTriId { get; set; }
}
