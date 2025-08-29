using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

[PrimaryKey("Factory", "Employee_ID", "Leave_code", "Leave_Date")]
public partial class HRMS_Att_Leave_Maintain
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
    /// 出勤代碼
    /// </summary>
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string Leave_code { get; set; }

    /// <summary>
    /// 請假日期
    /// </summary>
    [Key]
    [Column(TypeName = "date")]
    public DateTime Leave_Date { get; set; }

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
