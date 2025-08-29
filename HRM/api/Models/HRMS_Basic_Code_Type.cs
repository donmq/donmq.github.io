using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

public partial class HRMS_Basic_Code_Type
{
    /// <summary>
    /// 種類序號
    /// </summary>
    [Key]
    [StringLength(5)]
    [Unicode(false)]
    public string Type_Seq { get; set; }

    /// <summary>
    /// 種類名稱
    /// </summary>
    [StringLength(100)]
    public string Type_Name { get; set; }

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
