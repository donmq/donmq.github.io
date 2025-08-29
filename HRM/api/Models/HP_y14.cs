using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

[Keyless]
public partial class HP_y14
{
    [StringLength(2)]
    [Unicode(false)]
    public string tab { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string compy { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string depno { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string manuf { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string idno { get; set; }

    [StringLength(5)]
    [Unicode(false)]
    public string empno { get; set; }

    [StringLength(3)]
    [Unicode(false)]
    public string dept { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string sex { get; set; }

    [StringLength(36)]
    [Unicode(false)]
    public string name { get; set; }

    [StringLength(3)]
    [Unicode(false)]
    public string posit { get; set; }

    [StringLength(2)]
    [Unicode(false)]
    public string leve1 { get; set; }

    [StringLength(2)]
    [Unicode(false)]
    public string leve2 { get; set; }

    [Column(TypeName = "date")]
    public DateTime? birth { get; set; }

    [Column(TypeName = "date")]
    public DateTime? inday { get; set; }

    [Column(TypeName = "date")]
    public DateTime? outday { get; set; }

    [StringLength(3)]
    [Unicode(false)]
    public string outcd { get; set; }

    [StringLength(12)]
    [Unicode(false)]
    public string tel { get; set; }

    [StringLength(3)]
    [Unicode(false)]
    public string edubk { get; set; }

    [StringLength(36)]
    [Unicode(false)]
    public string jname { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string jposit { get; set; }

    [StringLength(80)]
    [Unicode(false)]
    public string jcause { get; set; }

    [StringLength(6)]
    [Unicode(false)]
    public string upusr { get; set; }

    [Column(TypeName = "date")]
    public DateTime? upday { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string odepno { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string omanuf { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string upcode { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string BIZ_FLAG { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? BIZ_TIME { get; set; }
}
