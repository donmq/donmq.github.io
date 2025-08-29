using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

[PrimaryKey("Cost_Year", "Company_Code", "Cost_Code")]
public partial class HRMS_Sal_SAPCostCenter
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
    [Required]
    [StringLength(10)]
    [Unicode(false)]
    public string Factory { get; set; }

    /// <summary>
    /// 公司代碼
    /// </summary>
    [Key]
    [StringLength(4)]
    [Unicode(false)]
    public string Company_Code { get; set; }

    /// <summary>
    /// 成本中心群組
    /// </summary>
    [Required]
    [StringLength(10)]
    [Unicode(false)]
    public string Group_Id { get; set; }

    /// <summary>
    /// 成本中心代碼
    /// </summary>
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string Cost_Code { get; set; }

    /// <summary>
    /// 中文說明
    /// </summary>
    [Required]
    [StringLength(50)]
    public string Code_Name { get; set; }

    /// <summary>
    /// 英文說明
    /// </summary>
    [Required]
    [StringLength(50)]
    public string Code_Name_EN { get; set; }

    /// <summary>
    /// 功能範圍_序號50
    /// </summary>
    [StringLength(10)]
    [Unicode(false)]
    public string Kind { get; set; }

    /// <summary>
    /// 利潤中心代碼
    /// </summary>
    [Required]
    [StringLength(10)]
    [Unicode(false)]
    public string Profit_Center { get; set; }

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
