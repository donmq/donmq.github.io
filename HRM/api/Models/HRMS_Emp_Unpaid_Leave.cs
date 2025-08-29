using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

[PrimaryKey("Division", "Factory", "Employee_ID", "Seq")]
public partial class HRMS_Emp_Unpaid_Leave
{
    /// <summary>
    /// 事業部
    /// </summary>
    [Key]
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
    /// 序號
    /// </summary>
    [Key]
    public int Seq { get; set; }

    /// <summary>
    /// 留停原因
    /// </summary>
    [Required]
    [StringLength(10)]
    [Unicode(false)]
    public string Leave_Reason { get; set; }

    /// <summary>
    /// 留停起始日
    /// </summary>
    [Column(TypeName = "date")]
    public DateTime Leave_Start { get; set; }

    /// <summary>
    /// 留停結束日
    /// </summary>
    [Column(TypeName = "date")]
    public DateTime Leave_End { get; set; }

    /// <summary>
    /// 保險繼續加保
    /// </summary>
    public bool Continuation_of_Insurance { get; set; }

    /// <summary>
    /// 年資保留
    /// </summary>
    public bool Seniority_Retention { get; set; }

    /// <summary>
    /// 年假年資保留
    /// </summary>
    public bool Annual_Leave_Seniority_Retention { get; set; }

    /// <summary>
    /// 生效狀態
    /// </summary>
    public bool Effective_Status { get; set; }

    /// <summary>
    /// 備註
    /// </summary>
    [StringLength(50)]
    public string Remark { get; set; }

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
