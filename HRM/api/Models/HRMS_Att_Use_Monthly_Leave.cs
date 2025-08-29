using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

[PrimaryKey("Factory", "Effective_Month", "Leave_Type", "Code")]
public partial class HRMS_Att_Use_Monthly_Leave
{
    /// <summary>
    /// 廠別
    /// </summary>
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string Factory { get; set; }

    /// <summary>
    /// 生效年月
    /// </summary>
    [Key]
    [Column(TypeName = "date")]
    public DateTime Effective_Month { get; set; }

    /// <summary>
    /// 1:出勤類 2:加班補助類
    /// </summary>
    [Key]
    [StringLength(2)]
    [Unicode(false)]
    public string Leave_Type { get; set; }

    /// <summary>
    /// 出勤代碼40 加班代碼42
    /// </summary>
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string Code { get; set; }

    /// <summary>
    /// 排序顯示
    /// </summary>
    public int Seq { get; set; }

    /// <summary>
    /// 月份累計顯示
    /// </summary>
    public bool Month_Total { get; set; }

    /// <summary>
    /// 年度累計顯示
    /// </summary>
    public bool? Year_Total { get; set; }

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
