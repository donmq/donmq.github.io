using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

[PrimaryKey("Factory", "Kind", "Department", "Code")]
public partial class HRMS_Sal_FinCategory
{
    /// <summary>
    /// 廠別
    /// </summary>
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string Factory { get; set; }

    /// <summary>
    /// 對應方式_序號63
    /// </summary>
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string Kind { get; set; }

    /// <summary>
    /// 部門
    /// </summary>
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string Department { get; set; }

    /// <summary>
    /// 職稱_序號3/權限身分別_序號4
    /// </summary>
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string Code { get; set; }

    /// <summary>
    /// 薪資歸屬類別_64
    /// </summary>
    [StringLength(10)]
    [Unicode(false)]
    public string Sortcod { get; set; }

    /// <summary>
    /// 異動者
    /// </summary>
    [Required]
    [StringLength(20)]
    [Unicode(false)]
    public string Update_By { get; set; }

    /// <summary>
    /// 異動日期
    /// </summary>
    [Column(TypeName = "datetime")]
    public DateTime Update_Time { get; set; }
}
