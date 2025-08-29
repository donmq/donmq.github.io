using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

[PrimaryKey("Division", "Factory", "Contract_Type", "Seq")]
public partial class HRMS_Emp_Contract_Type_Detail
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
    /// 合同類別
    /// </summary>
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string Contract_Type { get; set; }

    /// <summary>
    /// 序號
    /// </summary>
    [Key]
    public int Seq { get; set; }

    /// <summary>
    /// 排程頻率
    /// </summary>
    [Required]
    [StringLength(3)]
    [Unicode(false)]
    public string Schedule_Frequency { get; set; }

    /// <summary>
    /// 每月幾號
    /// </summary>
    public short? Day_Of_Month { get; set; }

    /// <summary>
    /// 預警規則
    /// </summary>
    [Required]
    [StringLength(3)]
    [Unicode(false)]
    public string Alert_Rules { get; set; }

    /// <summary>
    /// 到期前天數
    /// </summary>
    public short? Days_Before_Expiry_Date { get; set; }

    /// <summary>
    /// 月份範圍
    /// </summary>
    public short Month_Range { get; set; }

    /// <summary>
    /// 合同到期日(起)
    /// </summary>
    public short? Contract_Start { get; set; }

    /// <summary>
    /// 合同到期日(迄)
    /// </summary>
    public short? Contract_End { get; set; }

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
