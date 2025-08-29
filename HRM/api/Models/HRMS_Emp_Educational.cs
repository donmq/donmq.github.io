using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

[PrimaryKey("USER_GUID", "Degree", "Period_Start")]
public partial class HRMS_Emp_Educational
{
    /// <summary>
    /// 員工GUID
    /// </summary>
    [Key]
    [StringLength(50)]
    public string USER_GUID { get; set; }

    /// <summary>
    /// 學位
    /// </summary>
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string Degree { get; set; }

    /// <summary>
    /// 學制
    /// </summary>
    [Required]
    [StringLength(10)]
    [Unicode(false)]
    public string Academic_System { get; set; }

    /// <summary>
    /// 專業
    /// </summary>
    [Required]
    [StringLength(10)]
    [Unicode(false)]
    public string Major { get; set; }

    /// <summary>
    /// 學校
    /// </summary>
    [Required]
    [StringLength(100)]
    public string School { get; set; }

    /// <summary>
    /// 科系別　
    /// </summary>
    [Required]
    [StringLength(50)]
    public string Department { get; set; }

    /// <summary>
    /// 就讀期間起
    /// </summary>
    [Key]
    [StringLength(7)]
    [Unicode(false)]
    public string Period_Start { get; set; }

    /// <summary>
    /// 就讀期間迄
    /// </summary>
    [Required]
    [StringLength(7)]
    [Unicode(false)]
    public string Period_End { get; set; }

    /// <summary>
    /// 畢業
    /// </summary>
    public bool Graduation { get; set; }

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
