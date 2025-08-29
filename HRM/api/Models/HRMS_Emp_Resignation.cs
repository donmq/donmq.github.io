using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

[PrimaryKey("USER_GUID", "Division", "Factory", "Employee_ID")]
public partial class HRMS_Emp_Resignation
{
    /// <summary>
    /// 員工GUID
    /// </summary>
    [Key]
    [StringLength(50)]
    public string USER_GUID { get; set; }

    /// <summary>
    /// 事業部
    /// </summary>
    [Key]
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
    /// 工號
    /// </summary>
    [Key]
    [StringLength(16)]
    [Unicode(false)]
    public string Employee_ID { get; set; }

    /// <summary>
    /// 國籍
    /// </summary>
    [Required]
    [StringLength(10)]
    [Unicode(false)]
    public string Nationality { get; set; }

    /// <summary>
    /// 身分證號
    /// </summary>
    [Required]
    [StringLength(50)]
    public string Identification_Number { get; set; }

    /// <summary>
    /// 到職日期
    /// </summary>
    [Column(TypeName = "date")]
    public DateTime Onboard_Date { get; set; }

    /// <summary>
    /// 離職日期
    /// </summary>
    [Column(TypeName = "date")]
    public DateTime Resign_Date { get; set; }

    /// <summary>
    /// 離職類別
    /// </summary>
    [Required]
    [StringLength(10)]
    [Unicode(false)]
    public string Resignation_Type { get; set; }

    /// <summary>
    /// 離職原因
    /// </summary>
    [Required]
    [StringLength(10)]
    [Unicode(false)]
    public string Resign_Reason { get; set; }

    /// <summary>
    /// 備註
    /// </summary>
    [StringLength(255)]
    public string Remark { get; set; }

    /// <summary>
    /// 黑名單
    /// </summary>
    public bool? Blacklist { get; set; }

    /// <summary>
    /// 判定人員工號
    /// </summary>
    [StringLength(16)]
    [Unicode(false)]
    public string Verifier { get; set; }

    /// <summary>
    /// 判定人本地姓名
    /// </summary>
    [StringLength(100)]
    public string Verifier_Name { get; set; }

    /// <summary>
    /// 判定人職稱
    /// </summary>
    [StringLength(10)]
    [Unicode(false)]
    public string Verifier_Title { get; set; }

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
