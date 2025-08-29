using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

[Keyless]
public partial class HP_g26
{
    [StringLength(1)]
    [Unicode(false)]
    public string compy { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string depno { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string manuf { get; set; }

    [StringLength(6)]
    [Unicode(false)]
    public string yymm { get; set; }

    [StringLength(2)]
    [Unicode(false)]
    public string level { get; set; }

    [StringLength(4)]
    [Unicode(false)]
    public string speci { get; set; }

    [StringLength(2)]
    [Unicode(false)]
    public string fun { get; set; }

    [StringLength(6)]
    [Unicode(false)]
    public string flag { get; set; }

    [StringLength(6)]
    [Unicode(false)]
    public string upusr { get; set; }

    [Column(TypeName = "datetime")]
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
