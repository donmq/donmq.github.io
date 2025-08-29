using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

[PrimaryKey("Kind", "Code", "Language_Code")]
public partial class HRMS_SYS_Program_Language
{
    /// <summary>
    /// 類別
    /// </summary>
    [Key]
    [StringLength(1)]
    [Unicode(false)]
    public string Kind { get; set; }

    /// <summary>
    /// 目錄/程式代碼
    /// </summary>
    [Key]
    [StringLength(50)]
    [Unicode(false)]
    public string Code { get; set; }

    /// <summary>
    /// 語系代碼
    /// </summary>
    [Key]
    [StringLength(2)]
    [Unicode(false)]
    public string Language_Code { get; set; }

    /// <summary>
    /// 目錄/程式名稱
    /// </summary>
    [StringLength(100)]
    public string Name { get; set; }

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
