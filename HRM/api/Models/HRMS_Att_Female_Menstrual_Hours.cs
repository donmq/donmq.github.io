using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

[PrimaryKey("Factory", "Att_Month", "Breaks_Date", "Employee_ID", "Time_Start")]
public partial class HRMS_Att_Female_Menstrual_Hours
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
    /// 月份
    /// </summary>
    [Key]
    [Column(TypeName = "date")]
    public DateTime Att_Month { get; set; }

    /// <summary>
    /// 休息日期
    /// </summary>
    [Key]
    [Column(TypeName = "date")]
    public DateTime Breaks_Date { get; set; }

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
    /// 休息開始時間
    /// </summary>
    [Key]
    [StringLength(4)]
    [Unicode(false)]
    public string Time_Start { get; set; }

    /// <summary>
    /// 休息結束時間
    /// </summary>
    [Required]
    [StringLength(4)]
    [Unicode(false)]
    public string Time_End { get; set; }

    /// <summary>
    /// 休息時數
    /// </summary>
    [Column(TypeName = "decimal(10, 5)")]
    public decimal Breaks_Hours { get; set; }

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
