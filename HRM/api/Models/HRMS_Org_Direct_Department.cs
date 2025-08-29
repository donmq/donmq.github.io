using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

[PrimaryKey("Division", "Factory", "Department_Code", "Line_Code")]
public partial class HRMS_Org_Direct_Department
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
    /// 部門代碼
    /// </summary>
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string Department_Code { get; set; }

    /// <summary>
    /// 線別代碼
    /// </summary>
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string Line_Code { get; set; }

    [Required]
    [StringLength(10)]
    [Unicode(false)]
    public string Direct_Department_Attribute { get; set; }

    /// <summary>
    /// 異動者
    /// </summary>
    [Required]
    [StringLength(20)]
    [Unicode(false)]
    public string Update_By { get; set; }

    /// <summary>
    /// 異動日
    /// </summary>
    [Column(TypeName = "datetime")]
    public DateTime Update_Time { get; set; }
}
