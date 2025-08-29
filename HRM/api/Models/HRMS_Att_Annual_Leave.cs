using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

[PrimaryKey("Annual_Start", "Factory", "Employee_ID", "Leave_Code")]
public partial class HRMS_Att_Annual_Leave
{
    /// <summary>
    /// 員工GUID
    /// </summary>
    [Required]
    [StringLength(50)]
    public string USER_GUID { get; set; }

    /// <summary>
    /// 可休開始日
    /// </summary>
    [Key]
    [Column(TypeName = "date")]
    public DateTime Annual_Start { get; set; }

    /// <summary>
    /// 可休結束日
    /// </summary>
    [Column(TypeName = "date")]
    public DateTime Annual_End { get; set; }

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
    /// 出勤代碼40
    /// </summary>
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string Leave_Code { get; set; }

    /// <summary>
    /// 上期結轉時數
    /// </summary>
    [Column(TypeName = "decimal(10, 5)")]
    public decimal Previous_Hours { get; set; }

    /// <summary>
    /// 年度時數
    /// </summary>
    [Column(TypeName = "decimal(10, 5)")]
    public decimal Year_Hours { get; set; }

    /// <summary>
    /// 合計時數
    /// </summary>
    [Column(TypeName = "decimal(10, 5)")]
    public decimal Total_Hours { get; set; }

    /// <summary>
    /// 換算天數
    /// </summary>
    [Column(TypeName = "decimal(10, 5)")]
    public decimal Total_Days { get; set; }

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
