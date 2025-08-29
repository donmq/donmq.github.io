using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

[PrimaryKey("Factory", "Att_Month", "Employee_ID")]
public partial class HRMS_Att_Probation_Monthly
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
    /// 出勤月份
    /// </summary>
    [Key]
    [Column(TypeName = "date")]
    public DateTime Att_Month { get; set; }

    /// <summary>
    /// 工號
    /// </summary>
    [Key]
    [StringLength(16)]
    [Unicode(false)]
    public string Employee_ID { get; set; }

    /// <summary>
    /// 試用期識別_N.正式員工   Y.為試用期轉正
    /// </summary>
    [StringLength(1)]
    [Unicode(false)]
    public string Probation { get; set; }

    /// <summary>
    /// 部門
    /// </summary>
    [StringLength(10)]
    [Unicode(false)]
    public string Department { get; set; }

    /// <summary>
    /// 過帳碼
    /// </summary>
    public bool Pass { get; set; }

    /// <summary>
    /// 應上班天數
    /// </summary>
    [Column(TypeName = "decimal(10, 5)")]
    public decimal Salary_Days { get; set; }

    /// <summary>
    /// 實際上班天數_計薪天數
    /// </summary>
    [Column(TypeName = "decimal(10, 5)")]
    public decimal Actual_Days { get; set; }

    /// <summary>
    /// 權限身分別
    /// </summary>
    [Required]
    [StringLength(10)]
    [Unicode(false)]
    public string Permission_Group { get; set; }

    /// <summary>
    /// 薪資計別
    /// </summary>
    [Required]
    [StringLength(10)]
    [Unicode(false)]
    public string Salary_Type { get; set; }

    /// <summary>
    /// 遲到早退
    /// </summary>
    public int Delay_Early { get; set; }

    /// <summary>
    /// 未刷卡次
    /// </summary>
    public int No_Swip_Card { get; set; }

    /// <summary>
    /// 白班伙食次數
    /// </summary>
    public int? DayShift_Food { get; set; }

    /// <summary>
    /// 伙食費次數
    /// </summary>
    public int Food_Expenses { get; set; }

    /// <summary>
    /// 夜點費次數
    /// </summary>
    public int Night_Eat_Times { get; set; }

    /// <summary>
    /// 夜班伙食次數
    /// </summary>
    public int? NightShift_Food { get; set; }

    /// <summary>
    /// 離職Y
    /// </summary>
    [Required]
    [StringLength(1)]
    [Unicode(false)]
    public string Resign_Status { get; set; }

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
