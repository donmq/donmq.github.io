using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

[PrimaryKey("Factory", "Sal_Month", "Employee_ID", "AddDed_Type", "AddDed_Item")]
public partial class HRMS_Sal_AddDedItem_Monthly
{
    /// <summary>
    /// 員工GUID
    /// </summary>
    [Required]
    [StringLength(50)]
    public string USER_GUID { get; set; }

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
    /// 幣別
    /// </summary>
    [Required]
    [StringLength(10)]
    [Unicode(false)]
    public string Currency { get; set; }

    /// <summary>
    /// 金額
    /// </summary>
    public int Amount { get; set; }

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
