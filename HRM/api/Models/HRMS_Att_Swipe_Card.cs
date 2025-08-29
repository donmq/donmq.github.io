using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

[PrimaryKey("Factory", "Employee_ID", "Card_Time", "Card_Date")]
public partial class HRMS_Att_Swipe_Card
{
    /// <summary>
    /// 入考勤否
    /// </summary>
    [Required]
    [StringLength(1)]
    [Unicode(false)]
    public string Mark { get; set; }

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
    /// 班別
    /// </summary>
    [Required]
    [StringLength(10)]
    [Unicode(false)]
    public string Work_Shift_Type { get; set; }

    /// <summary>
    /// 時間
    /// </summary>
    [Key]
    [StringLength(4)]
    [Unicode(false)]
    public string Card_Time { get; set; }

    /// <summary>
    /// 日期
    /// </summary>
    [Key]
    [StringLength(4)]
    [Unicode(false)]
    public string Card_Date { get; set; }

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
