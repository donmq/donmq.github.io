using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

public partial class LoaiThuocTinh
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("LoaiThuocTinh")]
    [StringLength(50)]
    public string LoaiThuocTinh1 { get; set; }
}
