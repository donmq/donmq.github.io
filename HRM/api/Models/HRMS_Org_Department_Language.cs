using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

[PrimaryKey("Division", "Factory", "Department_Code", "Language_Code")]
public partial class HRMS_Org_Department_Language
{
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string Division { get; set; }

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
    /// 語系代碼
    /// </summary>
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string Language_Code { get; set; }

    /// <summary>
    /// 部門名稱
    /// </summary>
    [StringLength(100)]
    public string Name { get; set; }

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
