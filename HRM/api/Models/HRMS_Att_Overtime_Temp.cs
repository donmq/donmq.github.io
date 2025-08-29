using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

[PrimaryKey("Factory", "Overtime_Date", "Employee_ID")]
public partial class HRMS_Att_Overtime_Temp
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
    /// 日期
    /// </summary>
    [Key]
    [Column(TypeName = "date")]
    public DateTime Overtime_Date { get; set; }

    /// <summary>
    /// 工號
    /// </summary>
    [Key]
    [StringLength(16)]
    [Unicode(false)]
    public string Employee_ID { get; set; }

    /// <summary>
    /// 部門
    /// </summary>
    [StringLength(10)]
    [Unicode(false)]
    public string Department { get; set; }

    /// <summary>
    /// 班別
    /// </summary>
    [Required]
    [StringLength(10)]
    [Unicode(false)]
    public string Work_Shift_Type { get; set; }

    /// <summary>
    /// 加班開始
    /// </summary>
    [Required]
    [StringLength(4)]
    [Unicode(false)]
    public string Overtime_Start { get; set; }

    /// <summary>
    /// 加班結束
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
    /// 夜班加班時數
    /// </summary>
    [Column(TypeName = "decimal(10, 5)")]
    public decimal Night_Overtime_Hours { get; set; }

    /// <summary>
    /// 訓練時數
    /// </summary>
    [Column(TypeName = "decimal(10, 5)")]
    public decimal Training_Hours { get; set; }

    /// <summary>
    /// 夜點費次數
    /// </summary>
    public int Night_Eat_Times { get; set; }

    /// <summary>
    /// 假日否
    /// </summary>
    [Required]
    [StringLength(10)]
    [Unicode(false)]
    public string Holiday { get; set; }

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
