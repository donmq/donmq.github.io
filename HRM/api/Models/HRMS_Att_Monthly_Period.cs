using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

[PrimaryKey("Factory", "Att_Month")]
public partial class HRMS_Att_Monthly_Period
{
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
    /// 結算開始日
    /// </summary>
    [Column(TypeName = "date")]
    public DateTime Deadline_Start { get; set; }

    /// <summary>
    /// 結算結束日
    /// </summary>
    [Column(TypeName = "date")]
    public DateTime Deadline_End { get; set; }

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
