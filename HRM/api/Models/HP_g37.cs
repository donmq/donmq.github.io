using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

[Keyless]
public partial class HP_g37
{
    [StringLength(1)]
    [Unicode(false)]
    public string compy { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string manuf { get; set; }

    [StringLength(5)]
    [Unicode(false)]
    public string empno { get; set; }

    [Column(TypeName = "date")]
    public DateTime? ymd { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string squad { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string code { get; set; }

    [Column(TypeName = "decimal(18, 6)")]
    public decimal? dat { get; set; }

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
