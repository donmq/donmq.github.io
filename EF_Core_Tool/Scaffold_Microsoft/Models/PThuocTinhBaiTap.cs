using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

[PrimaryKey("IdbaiTap", "IdthuocTinhChinh")]
[Table("P_ThuocTinhBaiTap")]
public partial class PThuocTinhBaiTap
{
    [Key]
    [Column("IDBaiTap")]
    public int IdbaiTap { get; set; }

    [Key]
    [Column("IDThuocTinhChinh")]
    public int IdthuocTinhChinh { get; set; }
}
