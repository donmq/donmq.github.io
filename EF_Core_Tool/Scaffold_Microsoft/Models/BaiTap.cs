using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

public partial class BaiTap
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [StringLength(50)]
    public string TenBaiTap { get; set; }

    [Column("IDThuocTinh")]
    public int? IdthuocTinh { get; set; }

    public int? DoKho { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string PhanLoai { get; set; }
}
