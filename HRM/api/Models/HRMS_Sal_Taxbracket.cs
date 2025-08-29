using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

[PrimaryKey("Nation", "Tax_Code", "Effective_Month", "Tax_Level")]
public partial class HRMS_Sal_Taxbracket
{
    /// <summary>
    /// 國家_10
    /// </summary>
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string Nation { get; set; }

    /// <summary>
    /// 稅碼_53
    /// </summary>
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string Tax_Code { get; set; }

    /// <summary>
    /// 生效年月
    /// </summary>
    [Key]
    [Column(TypeName = "date")]
    public DateTime Effective_Month { get; set; }

    /// <summary>
    /// 級別
    /// </summary>
    [Key]
    public int Tax_Level { get; set; }

    /// <summary>
    /// 所得淨額-起
    /// </summary>
    [Column(TypeName = "decimal(16, 0)")]
    public decimal Income_Start { get; set; }

    /// <summary>
    /// 所得淨額-迄
    /// </summary>
    [Column(TypeName = "decimal(16, 0)")]
    public decimal Income_End { get; set; }

    /// <summary>
    /// 稅率
    /// </summary>
    [Column(TypeName = "decimal(6, 3)")]
    public decimal Rate { get; set; }

    /// <summary>
    /// 累進差額
    /// </summary>
    [Column(TypeName = "decimal(16, 0)")]
    public decimal Deduction { get; set; }

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
