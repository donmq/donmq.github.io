using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

[PrimaryKey("USER_GUID", "Seq")]
public partial class HRMS_Emp_External_Experience
{
    /// <summary>
    /// 員工GUID
    /// </summary>
    [Key]
    [StringLength(50)]
    public string USER_GUID { get; set; }

    /// <summary>
    /// 序號
    /// </summary>
    [Key]
    public int Seq { get; set; }

    /// <summary>
    /// 公司名稱
    /// </summary>
    [Required]
    [StringLength(100)]
    public string Company_Name { get; set; }

    /// <summary>
    /// 部門
    /// </summary>
    [StringLength(100)]
    public string Department { get; set; }

    /// <summary>
    /// 主管職
    /// </summary>
    public bool? Leadership_Role { get; set; }

    /// <summary>
    /// 職稱
    /// </summary>
    [StringLength(30)]
    public string Position_Title { get; set; }

    /// <summary>
    /// 任職期間起
    /// </summary>
    [Column(TypeName = "datetime")]
    public DateTime Tenure_Start { get; set; }

    /// <summary>
    /// 任職期間迄
    /// </summary>
    [Column(TypeName = "datetime")]
    public DateTime Tenure_End { get; set; }

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
