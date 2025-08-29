using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

public partial class HRMS_SYS_Directory
{
    /// <summary>
    /// 順序
    /// </summary>
    [Required]
    [StringLength(3)]
    [Unicode(false)]
    public string Seq { get; set; }

    /// <summary>
    /// 目錄代碼
    /// </summary>
    [Key]
    [StringLength(50)]
    [Unicode(false)]
    public string Directory_Code { get; set; }

    /// <summary>
    /// 目錄名稱
    /// </summary>
    [Required]
    [StringLength(100)]
    public string Directory_Name { get; set; }

    /// <summary>
    /// 上層目錄代碼
    /// </summary>
    [StringLength(50)]
    [Unicode(false)]
    public string Parent_Directory_Code { get; set; }

    /// <summary>
    /// 預設語系
    /// </summary>
    [StringLength(2)]
    [Unicode(false)]
    public string Language { get; set; }

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
