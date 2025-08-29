using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

[Keyless]
public partial class HP_g59c
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

    [StringLength(5)]
    [Unicode(false)]
    public string empno { get; set; }

    [StringLength(3)]
    [Unicode(false)]
    public string speci1 { get; set; }

    [StringLength(3)]
    [Unicode(false)]
    public string speci2 { get; set; }

    [Column(TypeName = "date")]
    public DateTime? speci2_date { get; set; }

    [Column(TypeName = "date")]
    public DateTime? date11 { get; set; }

    public short? weeks { get; set; }

    [Column(TypeName = "date")]
    public DateTime? date31 { get; set; }

    [Column(TypeName = "date")]
    public DateTime? predate5 { get; set; }

    [Column(TypeName = "date")]
    public DateTime? predate6 { get; set; }

    [Column(TypeName = "date")]
    public DateTime? predate7 { get; set; }

    [Column(TypeName = "date")]
    public DateTime? predate8 { get; set; }

    [Column(TypeName = "date")]
    public DateTime? predate9 { get; set; }

    [Column(TypeName = "date")]
    public DateTime? date5 { get; set; }

    [Column(TypeName = "date")]
    public DateTime? date6 { get; set; }

    [Column(TypeName = "date")]
    public DateTime? date7 { get; set; }

    [Column(TypeName = "date")]
    public DateTime? date8 { get; set; }

    [Column(TypeName = "date")]
    public DateTime? date9 { get; set; }

    [Column(TypeName = "date")]
    public DateTime? leadate5 { get; set; }

    [Column(TypeName = "date")]
    public DateTime? leadate6 { get; set; }

    [Column(TypeName = "date")]
    public DateTime? leadate7 { get; set; }

    [Column(TypeName = "date")]
    public DateTime? leadate8 { get; set; }

    [Column(TypeName = "date")]
    public DateTime? leadate9 { get; set; }

    [Column(TypeName = "date")]
    public DateTime? date10 { get; set; }

    [Column(TypeName = "date")]
    public DateTime? date18 { get; set; }

    [Column(TypeName = "date")]
    public DateTime? date12 { get; set; }

    [Column(TypeName = "date")]
    public DateTime? date13 { get; set; }

    [Column(TypeName = "date")]
    public DateTime? date14 { get; set; }

    [Column(TypeName = "date")]
    public DateTime? date15 { get; set; }

    [Column(TypeName = "date")]
    public DateTime? date16 { get; set; }

    [StringLength(60)]
    [Unicode(false)]
    public string remark { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string cls { get; set; }

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
