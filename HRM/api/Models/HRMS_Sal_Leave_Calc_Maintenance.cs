using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

[PrimaryKey("Factory", "Leave_Code")]
public partial class HRMS_Sal_Leave_Calc_Maintenance
{
    /// <summary>
    /// 廠別
    /// </summary>
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string Factory { get; set; }

    /// <summary>
    /// 假別
    /// </summary>
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string Leave_Code { get; set; }

    /// <summary>
    /// 扣薪比例
    /// </summary>
    [Column(TypeName = "decimal(4, 1)")]
    public decimal Salary_Rate { get; set; }

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
