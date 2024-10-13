using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

[Keyless]
[Table("P_ThuocTinhSangToi")]
public partial class PThuocTinhSangToi
{
    [Column("IDViTri")]
    public int IdviTri { get; set; }

    [Column("IDThuocTinhChinh")]
    public int IdthuocTinhChinh { get; set; }

    public bool LoaiThuocTinh { get; set; }
}
