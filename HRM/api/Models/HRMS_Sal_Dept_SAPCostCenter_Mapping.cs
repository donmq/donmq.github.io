using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

[PrimaryKey("Cost_Year", "Factory", "Department")]
public partial class HRMS_Sal_Dept_SAPCostCenter_Mapping
{
    /// <summary>
    /// 年度
    /// </summary>
    [Key]
    [Column(TypeName = "date")]
    public DateTime Cost_Year { get; set; }

    /// <summary>
    /// 廠別
    /// </summary>
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string Factory { get; set; }

    /// <summary>
    /// 成本中心代碼
    /// </summary>
    [Required]
    [StringLength(10)]
    [Unicode(false)]
    public string Cost_Code { get; set; }

    /// <summary>
    /// 部門
    /// </summary>
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string Department { get; set; }

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
