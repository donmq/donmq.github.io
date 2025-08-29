using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

public partial class HRMS_SYS_Program
{
    /// <summary>
    /// 程式代碼
    /// </summary>
    [Key]
    [StringLength(20)]
    [Unicode(false)]
    public string Program_Code { get; set; }

    /// <summary>
    /// 程式名稱
    /// </summary>
    [Required]
    [StringLength(100)]
    public string Program_Name { get; set; }

    /// <summary>
    /// 上層目錄代碼
    /// </summary>
    [Required]
    [StringLength(50)]
    [Unicode(false)]
    public string Parent_Directory_Code { get; set; }

    public int? Seq { get; set; }

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
