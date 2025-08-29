using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

[PrimaryKey("Factory", "Employee_ID", "Effective_Date")]
public partial class HRMS_Att_Work_Shift_Change
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
    /// 班別_舊
    /// </summary>
    [Required]
    [StringLength(10)]
    [Unicode(false)]
    public string Work_Shift_Type_Old { get; set; }

    /// <summary>
    /// 班別_新
    /// </summary>
    [Required]
    [StringLength(10)]
    [Unicode(false)]
    public string Work_Shift_Type_New { get; set; }

    /// <summary>
    /// 生效日期
    /// </summary>
    [Key]
    [Column(TypeName = "date")]
    public DateTime Effective_Date { get; set; }

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
