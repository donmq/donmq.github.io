using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

public partial class HRMS_Att_Swipecard_Set
{
    /// <summary>
    /// 廠別
    /// </summary>
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string Factory { get; set; }

    /// <summary>
    /// 工號位置起
    /// </summary>
    public int Employee_Start { get; set; }

    /// <summary>
    /// 工號位置迄
    /// </summary>
    public int Employee_End { get; set; }

    /// <summary>
    /// 時間位置起
    /// </summary>
    public int Time_Start { get; set; }

    /// <summary>
    /// 時間位置迄
    /// </summary>
    public int Time_End { get; set; }

    /// <summary>
    /// 日期位置起
    /// </summary>
    public int Date_Start { get; set; }

    /// <summary>
    /// 日期位置迄
    /// </summary>
    public int Date_End { get; set; }

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
