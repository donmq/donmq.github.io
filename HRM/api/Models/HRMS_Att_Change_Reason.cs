using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

[PrimaryKey("Factory", "Att_Date", "Employee_ID")]
public partial class HRMS_Att_Change_Reason
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
    /// 出勤日期
    /// </summary>
    [Key]
    [Column(TypeName = "date")]
    public DateTime Att_Date { get; set; }

    /// <summary>
    /// 工號
    /// </summary>
    [Key]
    [StringLength(16)]
    [Unicode(false)]
    public string Employee_ID { get; set; }

    /// <summary>
    /// 班別
    /// </summary>
    [Required]
    [StringLength(10)]
    [Unicode(false)]
    public string Work_Shift_Type { get; set; }

    /// <summary>
    /// 出勤代碼
    /// </summary>
    [Required]
    [StringLength(10)]
    [Unicode(false)]
    public string Leave_Code { get; set; }

    /// <summary>
    /// 出勤修改原因
    /// </summary>
    [Required]
    [StringLength(10)]
    [Unicode(false)]
    public string Reason_Code { get; set; }

    /// <summary>
    /// 原上班刷卡
    /// </summary>
    [Required]
    [StringLength(4)]
    [Unicode(false)]
    public string Clock_In { get; set; }

    /// <summary>
    /// 原下班刷卡
    /// </summary>
    [Required]
    [StringLength(4)]
    [Unicode(false)]
    public string Clock_Out { get; set; }

    /// <summary>
    /// 原加班上班
    /// </summary>
    [Required]
    [StringLength(4)]
    [Unicode(false)]
    public string Overtime_ClockIn { get; set; }

    /// <summary>
    /// 原加班下班
    /// </summary>
    [Required]
    [StringLength(4)]
    [Unicode(false)]
    public string Overtime_ClockOut { get; set; }

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
