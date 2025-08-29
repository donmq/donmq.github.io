using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

[PrimaryKey("Factory", "Employee_ID")]
public partial class HRMS_Sal_Master
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
    /// 職等
    /// </summary>
    [Column(TypeName = "decimal(4, 1)")]
    public decimal Position_Grade { get; set; }

    /// <summary>
    /// 職稱
    /// </summary>
    [Required]
    [StringLength(10)]
    [Unicode(false)]
    public string Position_Title { get; set; }

    /// <summary>
    /// 權限身分別
    /// </summary>
    [Required]
    [StringLength(10)]
    [Unicode(false)]
    public string Permission_Group { get; set; }

    /// <summary>
    /// 職務代理期間-Start
    /// </summary>
    [Column(TypeName = "date")]
    public DateTime? ActingPosition_Start { get; set; }

    /// <summary>
    /// 職務代理期間-END
    /// </summary>
    [Column(TypeName = "date")]
    public DateTime? ActingPosition_End { get; set; }

    /// <summary>
    /// 技術工別_25
    /// </summary>
    [StringLength(10)]
    [Unicode(false)]
    public string Technical_Type { get; set; }

    /// <summary>
    /// 專長類別_27
    /// </summary>
    [StringLength(10)]
    [Unicode(false)]
    public string Expertise_Category { get; set; }

    /// <summary>
    /// 薪資計別_序號9
    /// </summary>
    [Required]
    [StringLength(10)]
    [Unicode(false)]
    public string Salary_Type { get; set; }

    /// <summary>
    /// 薪資等級-等
    /// </summary>
    [Column(TypeName = "decimal(4, 1)")]
    public decimal Salary_Grade { get; set; }

    /// <summary>
    /// 薪資等級-級別
    /// </summary>
    [Column(TypeName = "decimal(4, 1)")]
    public decimal Salary_Level { get; set; }

    /// <summary>
    /// 幣別
    /// </summary>
    [Required]
    [StringLength(10)]
    [Unicode(false)]
    public string Currency { get; set; }

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
