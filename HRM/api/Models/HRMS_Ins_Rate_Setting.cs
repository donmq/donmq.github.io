using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

[PrimaryKey("Factory", "Effective_Month", "Permission_Group", "Insurance_Type")]
public partial class HRMS_Ins_Rate_Setting
{
    /// <summary>
    /// 廠別
    /// </summary>
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string Factory { get; set; }

    /// <summary>
    /// 生效年月
    /// </summary>
    [Key]
    [Column(TypeName = "date")]
    public DateTime Effective_Month { get; set; }

    /// <summary>
    /// 權限身分別_序號4
    /// </summary>
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string Permission_Group { get; set; }

    /// <summary>
    /// 保險類別_序號57
    /// </summary>
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string Insurance_Type { get; set; }

    /// <summary>
    /// 公司負擔比率
    /// </summary>
    [Column(TypeName = "decimal(6, 3)")]
    public decimal Employer_Rate { get; set; }

    /// <summary>
    /// 員工負擔比率
    /// </summary>
    [Column(TypeName = "decimal(6, 3)")]
    public decimal Employee_Rate { get; set; }

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
