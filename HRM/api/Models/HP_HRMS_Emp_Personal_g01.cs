using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

[Keyless]
public partial class HP_HRMS_Emp_Personal_g01
{
    [StringLength(1)]
    [Unicode(false)]
    public string flag { get; set; }

    [StringLength(5)]
    [Unicode(false)]
    public string empno { get; set; }

    [StringLength(5)]
    [Unicode(false)]
    public string cardno { get; set; }

    [StringLength(36)]
    [Unicode(false)]
    public string vname { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string cname { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string idno { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string compy { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string depno { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string manuf { get; set; }

    [StringLength(3)]
    [Unicode(false)]
    public string dept { get; set; }

    [StringLength(3)]
    [Unicode(false)]
    public string posit { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string sex { get; set; }

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

    [StringLength(60)]
    [Unicode(false)]
    public string addr { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string impr { get; set; }

    [StringLength(3)]
    [Unicode(false)]
    public string speci { get; set; }

    [StringLength(2)]
    [Unicode(false)]
    public string edubk { get; set; }

    [StringLength(3)]
    [Unicode(false)]
    public string sect { get; set; }

    [StringLength(6)]
    [Unicode(false)]
    public string graym { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string marry { get; set; }

    [StringLength(2)]
    [Unicode(false)]
    public string insur1 { get; set; }

    [StringLength(2)]
    [Unicode(false)]
    public string insur2 { get; set; }

    [Column(TypeName = "date")]
    public DateTime? insdat1 { get; set; }

    [Column(TypeName = "date")]
    public DateTime? insdat2 { get; set; }

    [Column(TypeName = "date")]
    public DateTime? out_saf1 { get; set; }

    [Column(TypeName = "date")]
    public DateTime? out_saf2 { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string sacode { get; set; }

    [StringLength(2)]
    [Unicode(false)]
    public string leve1 { get; set; }

    [StringLength(2)]
    [Unicode(false)]
    public string leve2 { get; set; }

    [StringLength(2)]
    [Unicode(false)]
    public string rtaf { get; set; }

    [StringLength(3)]
    [Unicode(false)]
    public string arri { get; set; }

    [StringLength(4)]
    [Unicode(false)]
    public string sharea { get; set; }

    [StringLength(3)]
    [Unicode(false)]
    public string doarea { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string squad { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string wkgrp { get; set; }

    [Column(TypeName = "date")]
    public DateTime? tinday { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string insno { get; set; }

    [StringLength(15)]
    [Unicode(false)]
    public string medno { get; set; }

    [StringLength(3)]
    [Unicode(false)]
    public string native { get; set; }

    [Column(TypeName = "date")]
    public DateTime? idday { get; set; }

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
