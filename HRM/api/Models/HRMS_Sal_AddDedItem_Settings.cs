using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

[PrimaryKey("Factory", "Permission_Group", "Salary_Type", "Effective_Month", "AddDed_Type", "AddDed_Item")]
public partial class HRMS_Sal_AddDedItem_Settings
{
    /// <summary>
    /// 廠別
    /// </summary>
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string Factory { get; set; }

    /// <summary>
    /// 權限身分別_序號4
    /// </summary>
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string Permission_Group { get; set; }

    /// <summary>
    /// 薪資計別_序號9
    /// </summary>
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string Salary_Type { get; set; }

    /// <summary>
    /// 生效年月
    /// </summary>
    [Key]
    [Column(TypeName = "date")]
    public DateTime Effective_Month { get; set; }

    /// <summary>
    /// 加扣類別_序號48
    /// </summary>
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string AddDed_Type { get; set; }

    /// <summary>
    /// 加扣項目_序號49
    /// </summary>
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string AddDed_Item { get; set; }

    /// <summary>
    /// 金額
    /// </summary>
    public int Amount { get; set; }
    /// <summary>
    /// 在職要列印固定項目
    /// </summary>
    public string Onjob_Print { get; set; }
    /// <summary>
    /// 離職要列印固定項目
    /// </summary>
    public string Resigned_Print { get; set; }
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
