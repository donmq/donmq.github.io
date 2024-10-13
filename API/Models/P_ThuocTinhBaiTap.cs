using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

[Table("P_ThuocTinhBaiTap")]
public partial class P_ThuocTinhBaiTap
{
    [Key]
    [Column("IDBaiTap")]
    public int IDBaiTap { get; set; }

    [Key]
    [Column("IDThuocTinhChinh")]
    public int IDThuocTinhChinh { get; set; }
}
