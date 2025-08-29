using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

[PrimaryKey("Factory", "Employee_ID", "Declaration_Month", "Benefits_Kind", "Benefits_Start")]
public partial class HRMS_Ins_Benefits_Maintain
{
    /// <summary>
    /// 員工GUID
    /// </summary>
    [Required]
    [StringLength(50)]
    public string USER_GUID { get; set; }

    /// <summary>
    /// 廠別
    /// </summary>
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string Factory { get; set; }

    /// <summary>
    /// 工號
    /// </summary>
    [Key]
    [StringLength(16)]
    [Unicode(false)]
    public string Employee_ID { get; set; }

    /// <summary>
    /// 申報年月
    /// </summary>
    [Key]
    [Column(TypeName = "date")]
    public DateTime Declaration_Month { get; set; }

    /// <summary>
    /// 申報次數
    /// </summary>
    public short Declaration_Seq { get; set; }

    /// <summary>
    /// 疾病類別_序號58
    /// </summary>
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string Benefits_Kind { get; set; }

    /// <summary>
    /// 特殊工種Y/N
    /// </summary>
    [Required]
    [StringLength(1)]
    [Unicode(false)]
    public string Special_Work_Type { get; set; }

    /// <summary>
    /// 小孩出生日期
    /// </summary>
    [Column(TypeName = "date")]
    public DateTime? Birth_Child { get; set; }

    /// <summary>
    /// 開始日期
    /// </summary>
    [Key]
    [Column(TypeName = "date")]
    public DateTime Benefits_Start { get; set; }

    /// <summary>
    /// 結束日期
    /// </summary>
    [Column(TypeName = "date")]
    public DateTime Benefits_End { get; set; }

    /// <summary>
    /// 疾病編號
    /// </summary>
    [StringLength(10)]
    [Unicode(false)]
    public string Benefits_Num { get; set; }

    /// <summary>
    /// 天數
    /// </summary>
    public short Total_Days { get; set; }

    /// <summary>
    /// 總金額
    /// </summary>
    public int Amt { get; set; }

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
