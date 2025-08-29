using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

[PrimaryKey("Factory", "Work_Shift_Type", "Effective_Month", "Overtime_Start")]
public partial class HRMS_Att_Overtime_Parameter
{
    /// <summary>
    /// 事業部
    /// </summary>
    [Required]
    [StringLength(10)]
    [Unicode(false)]
    public string Division { get; set; }

    /// <summary>
    /// 廠別
    /// </summary>
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string Factory { get; set; }

    /// <summary>
    /// 班別
    /// </summary>
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string Work_Shift_Type { get; set; }

    /// <summary>
    /// 生效月份
    /// </summary>
    [Key]
    [Column(TypeName = "date")]
    public DateTime Effective_Month { get; set; }

    /// <summary>
    /// 加班時間-起
    /// </summary>
    [Key]
    [StringLength(4)]
    [Unicode(false)]
    public string Overtime_Start { get; set; }

    /// <summary>
    /// 加班時間-訖
    /// </summary>
    [Required]
    [StringLength(4)]
    [Unicode(false)]
    public string Overtime_End { get; set; }

    /// <summary>
    /// 加班時數
    /// </summary>
    [Column(TypeName = "decimal(10, 5)")]
    public decimal Overtime_Hours { get; set; }

    /// <summary>
    /// 夜班上班時數
    /// </summary>
    [Column(TypeName = "decimal(10, 5)")]
    public decimal Night_Hours { get; set; }

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
