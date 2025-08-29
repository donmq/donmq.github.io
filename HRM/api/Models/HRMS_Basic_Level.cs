using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

[PrimaryKey("Level", "Level_Code")]
public partial class HRMS_Basic_Level
{
    /// <summary>
    /// 職等
    /// </summary>
    [Key]
    [Column(TypeName = "decimal(4, 1)")]
    public decimal Level { get; set; }

    /// <summary>
    /// 職稱代碼
    /// </summary>
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string Level_Code { get; set; }

    /// <summary>
    /// 類別代碼
    /// </summary>
    [StringLength(1)]
    [Unicode(false)]
    public string Type_Code { get; set; }

    public bool IsActive { get; set; }

    /// <summary>
    /// 異動者
    /// </summary>
    [StringLength(20)]
    [Unicode(false)]
    public string Update_By { get; set; }

    /// <summary>
    /// 異動日
    /// </summary>
    [Column(TypeName = "datetime")]
    public DateTime? Update_Time { get; set; }
}
