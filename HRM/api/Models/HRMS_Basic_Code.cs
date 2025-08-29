using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

[PrimaryKey("Type_Seq", "Code")]
public partial class HRMS_Basic_Code
{
    /// <summary>
    /// 種類序號
    /// </summary>
    [Key]
    [StringLength(5)]
    [Unicode(false)]
    public string Type_Seq { get; set; }

    /// <summary>
    /// 代碼
    /// </summary>
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string Code { get; set; }

    /// <summary>
    /// 代碼名稱
    /// </summary>
    [StringLength(100)]
    public string Code_Name { get; set; }

    /// <summary>
    /// 字串一
    /// </summary>
    [StringLength(255)]
    public string Char1 { get; set; }

    /// <summary>
    /// 字串二
    /// </summary>
    [StringLength(255)]
    public string Char2 { get; set; }

    /// <summary>
    /// 日期一
    /// </summary>
    [Column(TypeName = "date")]
    public DateTime? Date1 { get; set; }

    /// <summary>
    /// 日期二
    /// </summary>
    [Column(TypeName = "date")]
    public DateTime? Date2 { get; set; }

    /// <summary>
    /// 日期三
    /// </summary>
    [Column(TypeName = "date")]
    public DateTime? Date3 { get; set; }

    /// <summary>
    /// 整數一
    /// </summary>
    public int? Int1 { get; set; }

    /// <summary>
    /// 整數二
    /// </summary>
    public int? Int2 { get; set; }

    /// <summary>
    /// 整數三
    /// </summary>
    public int? Int3 { get; set; }

    /// <summary>
    /// 數值一
    /// </summary>
    [Column(TypeName = "decimal(16, 5)")]
    public decimal? Decimal1 { get; set; }

    /// <summary>
    /// 數值二
    /// </summary>
    [Column(TypeName = "decimal(16, 5)")]
    public decimal? Decimal2 { get; set; }

    /// <summary>
    /// 數值三
    /// </summary>
    [Column(TypeName = "decimal(16, 5)")]
    public decimal? Decimal3 { get; set; }

    /// <summary>
    /// 備註說明
    /// </summary>
    public string Remark { get; set; }

    /// <summary>
    /// 備註碼
    /// </summary>
    [StringLength(50)]
    public string Remark_Code { get; set; }

    /// <summary>
    /// 啟用：1 啟用 0停用
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// 異動者
    /// </summary>
    [StringLength(20)]
    [Unicode(false)]
    public string Update_By { get; set; }

    /// <summary>
    /// 異動日
    /// </summary>
    [Column(TypeName = "datetime")]
    public DateTime? Update_Time { get; set; }

    /// <summary>
    /// 序號
    /// </summary>
    [StringLength(3)]
    [Unicode(false)]
    public string Seq { get; set; }
}
