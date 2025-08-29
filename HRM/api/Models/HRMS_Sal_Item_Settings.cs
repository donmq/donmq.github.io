using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

[PrimaryKey("Factory", "Permission_Group", "Salary_Type", "Effective_Month", "Salary_Item")]
public partial class HRMS_Sal_Item_Settings
{
    /// <summary>
    /// 廠別
    /// </summary>
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string Factory { get; set; }

    /// <summary>
    /// 權限身分別_序號4
    /// </summary>
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string Permission_Group { get; set; }

    /// <summary>
    /// 薪資計別_序號9
    /// </summary>
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string Salary_Type { get; set; }

    /// <summary>
    /// 生效年月
    /// </summary>
    [Key]
    [Column(TypeName = "date")]
    public DateTime Effective_Month { get; set; }

    /// <summary>
    /// 計薪天數
    /// </summary>
    public short Salary_Days { get; set; }

    /// <summary>
    /// 序號_排序使用
    /// </summary>
    public int Seq { get; set; }

    /// <summary>
    /// 薪資項目_序號45
    /// </summary>
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string Salary_Item { get; set; }

    /// <summary>
    /// 薪資類別_序號46
    /// </summary>
    [Required]
    [StringLength(10)]
    [Unicode(false)]
    public string Kind { get; set; }

    /// <summary>
    /// 是否計算保險
    /// </summary>
    [Required]
    [StringLength(1)]
    [Unicode(false)]
    public string Insurance { get; set; }

    /// <summary>
    /// 金額
    /// </summary>
    public int Amount { get; set; }

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
