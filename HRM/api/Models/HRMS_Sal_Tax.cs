using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

[PrimaryKey("Factory", "Sal_Month", "Employee_ID")]
public partial class HRMS_Sal_Tax
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
    /// 部門
    /// </summary>
    [Required]
    [StringLength(10)]
    [Unicode(false)]
    public string Department { get; set; }

    /// <summary>
    /// 幣別
    /// </summary>
    [Required]
    [StringLength(10)]
    [Unicode(false)]
    public string Currency { get; set; }

    /// <summary>
    /// 扶養人數
    /// </summary>
    public short Num_Dependents { get; set; }

    /// <summary>
    /// 正項合計
    /// </summary>
    public int Add_Total { get; set; }

    /// <summary>
    /// 扣項合計
    /// </summary>
    public int Ded_Total { get; set; }

    /// <summary>
    /// 應稅薪資
    /// </summary>
    public int Salary_Amt { get; set; }

    /// <summary>
    /// 當月所得稅率
    /// </summary>
    [Column(TypeName = "decimal(6, 3)")]
    public decimal Rate { get; set; }

    /// <summary>
    /// 所得稅金額_等同HRMS_Sal_Monthly.Tax
    /// </summary>
    public int Tax { get; set; }

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
