using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

[PrimaryKey("Division", "Factory", "Employee_ID", "Seq")]
public partial class HRMS_Emp_Contract_Management
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
    /// 合同類別
    /// </summary>
    [Required]
    [StringLength(10)]
    [Unicode(false)]
    public string Contract_Type { get; set; }

    /// <summary>
    /// 合同到期日(起)
    /// </summary>
    [Column(TypeName = "date")]
    public DateTime Contract_Start { get; set; }

    /// <summary>
    /// 合同到期日(迄)
    /// </summary>
    [Column(TypeName = "date")]
    public DateTime Contract_End { get; set; }

    /// <summary>
    /// 生效狀態
    /// </summary>
    public bool Effective_Status { get; set; }

    /// <summary>
    /// 試用起始日
    /// </summary>
    [Column(TypeName = "date")]
    public DateTime? Probation_Start { get; set; }

    /// <summary>
    /// 試用到期日
    /// </summary>
    [Column(TypeName = "date")]
    public DateTime? Probation_End { get; set; }

    /// <summary>
    /// 到期維護
    /// </summary>
    [StringLength(10)]
    [Unicode(false)]
    public string Assessment_Result { get; set; }

    /// <summary>
    /// 延長至
    /// </summary>
    [Column(TypeName = "date")]
    public DateTime? Extend_to { get; set; }

    /// <summary>
    /// 原因
    /// </summary>
    [StringLength(100)]
    public string Reason { get; set; }

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
