using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

[PrimaryKey("Division", "Factory", "Employee_ID")]
public partial class HRMS_Emp_Group
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
    /// 線別
    /// </summary>
    [Required]
    [StringLength(10)]
    [Unicode(false)]
    public string Production_Line { get; set; }

    /// <summary>
    /// 技術工別
    /// </summary>
    [Required]
    [StringLength(10)]
    [Unicode(false)]
    public string Technical_Type { get; set; }

    /// <summary>
    /// 績效類別
    /// </summary>
    [Required]
    [StringLength(10)]
    [Unicode(false)]
    public string Performance_Category { get; set; }

    /// <summary>
    /// 專長類別
    /// </summary>
    [Required]
    [StringLength(10)]
    [Unicode(false)]
    public string Expertise_Category { get; set; }

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
