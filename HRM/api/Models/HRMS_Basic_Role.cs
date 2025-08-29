using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

[PrimaryKey("Role", "Factory")]
public partial class HRMS_Basic_Role
{
    /// <summary>
    /// 角色
    /// </summary>
    [Key]
    [StringLength(20)]
    [Unicode(false)]
    public string Role { get; set; }

    /// <summary>
    /// 說明
    /// </summary>
    [StringLength(50)]
    [Unicode(false)]
    public string Description { get; set; }

    /// <summary>
    /// 廠別
    /// </summary>
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string Factory { get; set; }

    /// <summary>
    /// 薪資計別
    /// </summary>
    [Required]
    [StringLength(5)]
    [Unicode(false)]
    public string Permission_Group { get; set; }

    /// <summary>
    /// 職等-起
    /// </summary>
    [Column(TypeName = "decimal(4, 1)")]
    public decimal Level_Start { get; set; }

    /// <summary>
    /// 職等-迄
    /// </summary>
    [Column(TypeName = "decimal(4, 1)")]
    public decimal Level_End { get; set; }

    /// <summary>
    /// 直/間接人工：1 直接人工 2 間接人工 3 不區分
    /// </summary>
    [Required]
    [StringLength(5)]
    [Unicode(false)]
    public string Direct { get; set; }

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
