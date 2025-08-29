using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

[Keyless]
public partial class HP_g36
{
    [StringLength(1)]
    [Unicode(false)]
    public string compy { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string manuf { get; set; }

    [Column(TypeName = "date")]
    public DateTime? ymd { get; set; }

    [StringLength(5)]
    [Unicode(false)]
    public string empno { get; set; }

    [StringLength(3)]
    [Unicode(false)]
    public string dept { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string holi { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string squad { get; set; }

    [StringLength(4)]
    [Unicode(false)]
    public string shm { get; set; }

    [StringLength(4)]
    [Unicode(false)]
    public string ehm { get; set; }

    [Column(TypeName = "decimal(18, 6)")]
    public decimal? ovhrd { get; set; }

    [Column(TypeName = "decimal(18, 6)")]
    public decimal? ovhrn { get; set; }

    [Column(TypeName = "decimal(18, 6)")]
    public decimal? ovhrnt { get; set; }

    [Column(TypeName = "decimal(18, 6)")]
    public decimal? ovhrc { get; set; }

    [Column(TypeName = "decimal(18, 6)")]
    public decimal? ovhrt { get; set; }

    [StringLength(4)]
    [Unicode(false)]
    public string hm { get; set; }

    public int? nosh { get; set; }

    [StringLength(6)]
    [Unicode(false)]
    public string upusr { get; set; }

    [Column(TypeName = "date")]
    public DateTime? upday { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string upcode { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string BIZ_FLAG { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? BIZ_TIME { get; set; }
}
