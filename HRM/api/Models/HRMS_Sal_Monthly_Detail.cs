using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

[PrimaryKey("Factory", "Sal_Month", "Employee_ID", "Type_Seq", "AddDed_Type", "Item")]
public partial class HRMS_Sal_Monthly_Detail
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
    /// 代碼的序號，存放45/42/49 等序號
    /// </summary>
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string Type_Seq { get; set; }

    /// <summary>
    /// 加扣類別_序號48 
    /// </summary>
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string AddDed_Type { get; set; }

    /// <summary>
    /// 薪資項目45/加班補助類 42 /加扣項 49
    /// </summary>
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string Item { get; set; }

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
