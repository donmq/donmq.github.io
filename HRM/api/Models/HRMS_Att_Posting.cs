using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

[PrimaryKey("Factory", "Posting_Date", "Posting_Time")]
public partial class HRMS_Att_Posting
{
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
    [Column(TypeName = "date")]
    public DateTime Att_Date { get; set; }

    /// <summary>
    /// 過帳日期
    /// </summary>
    [Key]
    [Column(TypeName = "date")]
    public DateTime Posting_Date { get; set; }

    /// <summary>
    /// 過帳時間
    /// </summary>
    [Key]
    [Precision(0)]
    public TimeSpan Posting_Time { get; set; }

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
