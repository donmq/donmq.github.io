using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

public partial class HRMS_SYS_Language
{
    /// <summary>
    /// 語系代碼
    /// </summary>
    [Key]
    [StringLength(2)]
    [Unicode(false)]
    public string Language_Code { get; set; }

    /// <summary>
    /// 語系名稱
    /// </summary>
    [StringLength(50)]
    [Unicode(false)]
    public string Language_Name { get; set; }

    /// <summary>
    /// 啟用：1 啟用 0停用
    /// </summary>
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
