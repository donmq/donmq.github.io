using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

[PrimaryKey("Factory", "Sal_Month", "Employee_ID")]
public partial class HRMS_Sal_Close
{
    /// <summary>
    /// 員工GUID
    /// </summary>
    [Required]
    [StringLength(50)]
    public string USER_GUID { get; set; }

    /// <summary>
    /// 事業部
    /// </summary>
    [Required]
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
    /// 薪資年月
    /// </summary>
    [Key]
    [Column(TypeName = "date")]
    public DateTime Sal_Month { get; set; }

    /// <summary>
    /// 工號
    /// </summary>
    [Key]
    [StringLength(16)]
    [Unicode(false)]
    public string Employee_ID { get; set; }

    /// <summary>
    /// 權限身分別_序號4
    /// </summary>
    [Required]
    [StringLength(10)]
    [Unicode(false)]
    public string Permission_Group { get; set; }

    /// <summary>
    /// 關帳狀態
    /// </summary>
    [Required]
    [StringLength(1)]
    [Unicode(false)]
    public string Close_Status { get; set; }

    /// <summary>
    /// 關帳日期
    /// </summary>
    [Column(TypeName = "date")]
    public DateTime? Close_End { get; set; }

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
