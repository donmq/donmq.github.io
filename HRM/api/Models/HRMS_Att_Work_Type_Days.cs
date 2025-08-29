using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

[PrimaryKey("Factory", "Work_Type")]
public partial class HRMS_Att_Work_Type_Days
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
    /// 工種/職務_代碼5
    /// </summary>
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string Work_Type { get; set; }

    /// <summary>
    /// 可加休年假天數
    /// </summary>
    [Column(TypeName = "decimal(10, 5)")]
    public decimal Annual_leave_days { get; set; }

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
