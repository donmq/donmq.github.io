using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

[PrimaryKey("Division", "Factory", "Att_Date")]
public partial class HRMS_Att_Calendar
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
    /// 類別代碼39
    /// </summary>
    [Required]
    [StringLength(10)]
    [Unicode(false)]
    public string Type_Code { get; set; }

    /// <summary>
    /// 說明
    /// </summary>
    [Required]
    [StringLength(50)]
    public string Describe { get; set; }

    /// <summary>
    /// 停用原因
    /// </summary>
    [StringLength(50)]
    public string Reason { get; set; }

    /// <summary>
    /// 假日/重要日期
    /// </summary>
    [Key]
    [Column(TypeName = "date")]
    public DateTime Att_Date { get; set; }

    public bool? Effective_State { get; set; }

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
