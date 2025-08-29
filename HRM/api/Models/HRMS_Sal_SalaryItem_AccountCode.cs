using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

[PrimaryKey("Factory", "Salary_Item", "DC_Code")]
public partial class HRMS_Sal_SalaryItem_AccountCode
{
    /// <summary>
    /// 廠別
    /// </summary>
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string Factory { get; set; }

    /// <summary>
    /// 薪資項目_序號45
    /// </summary>
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string Salary_Item { get; set; }

    /// <summary>
    /// 主科目
    /// </summary>
    [Required]
    [StringLength(4)]
    [Unicode(false)]
    public string Main_Acc { get; set; }

    /// <summary>
    /// 子科目
    /// </summary>
    [Required]
    [StringLength(4)]
    [Unicode(false)]
    public string Sub_Acc { get; set; }

    /// <summary>
    /// 借D/貸C
    /// </summary>
    [Key]
    [StringLength(1)]
    [Unicode(false)]
    public string DC_Code { get; set; }

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
