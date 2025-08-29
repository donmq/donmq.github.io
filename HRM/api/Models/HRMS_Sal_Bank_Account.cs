using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

[PrimaryKey("Factory", "Employee_ID")]
public partial class HRMS_Sal_Bank_Account
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
    /// 工號
    /// </summary>
    [Key]
    [StringLength(16)]
    [Unicode(false)]
    public string Employee_ID { get; set; }

    /// <summary>
    /// 銀行代碼
    /// </summary>
    [Required]
    [StringLength(10)]
    [Unicode(false)]
    public string Bank_Code { get; set; }

    /// <summary>
    /// 帳號
    /// </summary>
    [Required]
    [StringLength(20)]
    [Unicode(false)]
    public string BankNo { get; set; }

    /// <summary>
    /// 日期
    /// </summary>
    [Column(TypeName = "date")]
    public DateTime Create_Date { get; set; }

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
