using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

[PrimaryKey("Factory", "Sal_Month", "Employee_ID", "Probation")]
public partial class HRMS_Sal_Probation_Monthly
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
    /// 試用期狀態識別
    /// </summary>
    [Key]
    [StringLength(1)]
    [Unicode(false)]
    public string Probation { get; set; }

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
    /// 權限身分別_序號4
    /// </summary>
    [Required]
    [StringLength(10)]
    [Unicode(false)]
    public string Permission_Group { get; set; }

    /// <summary>
    /// 薪資計別_序號9
    /// </summary>
    [Required]
    [StringLength(10)]
    [Unicode(false)]
    public string Salary_Type { get; set; }

    /// <summary>
    /// 薪資 Y.鎖定 N未鎖定
    /// </summary>
    [Required]
    [StringLength(1)]
    [Unicode(false)]
    public string Lock { get; set; }

    /// <summary>
    /// 銀行轉帳
    /// </summary>
    [Required]
    [StringLength(1)]
    [Unicode(false)]
    public string BankTransfer { get; set; }

    /// <summary>
    /// 所得稅金額_扣項
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
