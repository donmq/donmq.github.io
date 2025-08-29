using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

[PrimaryKey("Factory", "Work_Shift_Type", "Week")]
public partial class HRMS_Att_Work_Shift
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
    /// 班別_代碼41
    /// </summary>
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string Work_Shift_Type { get; set; }

    /// <summary>
    /// 星期別
    /// </summary>
    [Key]
    [StringLength(1)]
    [Unicode(false)]
    public string Week { get; set; }

    /// <summary>
    /// 上班
    /// </summary>
    [Required]
    [StringLength(4)]
    [Unicode(false)]
    public string Clock_In { get; set; }

    /// <summary>
    /// 下班
    /// </summary>
    [Required]
    [StringLength(4)]
    [Unicode(false)]
    public string Clock_Out { get; set; }

    /// <summary>
    /// 加班上班
    /// </summary>
    [StringLength(4)]
    [Unicode(false)]
    public string Overtime_ClockIn { get; set; }

    /// <summary>
    /// 加班下班
    /// </summary>
    [StringLength(4)]
    [Unicode(false)]
    public string Overtime_ClockOut { get; set; }

    /// <summary>
    /// 午餐時間-起
    /// </summary>
    [Required]
    [StringLength(4)]
    [Unicode(false)]
    public string Lunch_Start { get; set; }

    /// <summary>
    /// 午餐時間-訖
    /// </summary>
    [Required]
    [StringLength(4)]
    [Unicode(false)]
    public string Lunch_End { get; set; }

    /// <summary>
    /// 跨日
    /// </summary>
    [Required]
    [StringLength(2)]
    [Unicode(false)]
    public string Overnight { get; set; }

    /// <summary>
    /// 工作時數
    /// </summary>
    [Column(TypeName = "decimal(10, 5)")]
    public decimal Work_Hours { get; set; }

    /// <summary>
    /// 工作天數
    /// </summary>
    [Column(TypeName = "decimal(10, 5)")]
    public decimal Work_Days { get; set; }

    /// <summary>
    /// 生效狀態
    /// </summary>
    public bool Effective_State { get; set; }

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
