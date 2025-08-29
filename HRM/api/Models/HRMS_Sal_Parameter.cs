using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

[PrimaryKey("Factory", "Seq", "Code")]
public partial class HRMS_Sal_Parameter
{
    /// <summary>
    /// 廠別
    /// </summary>
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string Factory { get; set; }

    /// <summary>
    /// 序號56裡的代碼(大類)
    /// </summary>
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string Seq { get; set; }

    /// <summary>
    /// 代碼(小類)
    /// </summary>
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string Code { get; set; }

    /// <summary>
    /// 代碼名稱
    /// </summary>
    [Required]
    [StringLength(50)]
    public string Code_Name { get; set; }

    /// <summary>
    /// 次數
    /// </summary>
    public int? Num_Times { get; set; }

    /// <summary>
    /// 金額或比率
    /// </summary>
    [Column(TypeName = "decimal(16, 4)")]
    public decimal Code_Amt { get; set; }

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
