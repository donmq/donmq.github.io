using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

[PrimaryKey("Factory", "Att_Month", "Employee_ID", "Leave_Type", "Leave_Code")]
public partial class HRMS_Att_Monthly_Detail
{
    /// <summary>
    /// 員工GUID
    /// </summary>
    [Required]
    [StringLength(50)]
    public string USER_GUID { get; set; }

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
    /// 出勤月份
    /// </summary>
    [Key]
    [Column(TypeName = "date")]
    public DateTime Att_Month { get; set; }

    /// <summary>
    /// 工號
    /// </summary>
    [Key]
    [StringLength(16)]
    [Unicode(false)]
    public string Employee_ID { get; set; }

    /// <summary>
    /// 1:出勤類 2:加班補助類
    /// </summary>
    [Key]
    [StringLength(2)]
    [Unicode(false)]
    public string Leave_Type { get; set; }

    /// <summary>
    /// 出勤代碼40
    /// </summary>
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string Leave_Code { get; set; }

    /// <summary>
    /// 天數
    /// </summary>
    [Column(TypeName = "decimal(10, 5)")]
    public decimal Days { get; set; }

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
