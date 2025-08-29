using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

[PrimaryKey("Division", "Factory", "Department_Code")]
public partial class HRMS_Org_Department
{
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
    /// 中心代碼
    /// </summary>
    [Required]
    [StringLength(4)]
    [Unicode(false)]
    public string Center_Code { get; set; }

    /// <summary>
    /// 組織層級
    /// </summary>
    [Required]
    [StringLength(5)]
    [Unicode(false)]
    public string Org_Level { get; set; }

    /// <summary>
    /// 部門代碼
    /// </summary>
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string Department_Code { get; set; }

    /// <summary>
    /// 部門名稱
    /// </summary>
    [Required]
    [StringLength(50)]
    public string Department_Name { get; set; }

    /// <summary>
    /// 上一層部門代碼
    /// </summary>
    [StringLength(3)]
    [Unicode(false)]
    public string Upper_Department { get; set; }

    /// <summary>
    /// 部門屬性：Directly 直屬；Staff 幕僚；Non-Directly 非直屬。
    /// </summary>
    [Required]
    [StringLength(15)]
    [Unicode(false)]
    public string Attribute { get; set; }

    /// <summary>
    /// 虛擬歸屬部門代碼
    /// </summary>
    [StringLength(10)]
    [Unicode(false)]
    public string Virtual_Department { get; set; }

    /// <summary>
    /// 啟用
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// 部門主管工號
    /// </summary>
    [StringLength(12)]
    [Unicode(false)]
    public string Supervisor_Employee_ID { get; set; }

    /// <summary>
    /// 主管類型：A Formal正式；B Deputy 代理；C Adjunction兼任；D Informal非正式。
    /// </summary>
    [StringLength(1)]
    [Unicode(false)]
    public string Supervisor_Type { get; set; }

    /// <summary>
    /// 編制人數
    /// </summary>
    public int? Approved_Headcount { get; set; }

    /// <summary>
    /// 成本中心編號
    /// </summary>
    [Required]
    [StringLength(8)]
    [Unicode(false)]
    public string Cost_Center { get; set; }

    /// <summary>
    /// 生效日
    /// </summary>
    [Column(TypeName = "date")]
    public DateTime Effective_Date { get; set; }

    /// <summary>
    /// 失效日
    /// </summary>
    [Column(TypeName = "date")]
    public DateTime? Expiration_Date { get; set; }

    /// <summary>
    /// 異動者
    /// </summary>
    [Required]
    [StringLength(20)]
    [Unicode(false)]
    public string Update_By { get; set; }

    /// <summary>
    /// 異動日
    /// </summary>
    [Column(TypeName = "datetime")]
    public DateTime Update_Time { get; set; }
}
