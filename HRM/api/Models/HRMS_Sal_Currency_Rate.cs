using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

[PrimaryKey("Rate_Month", "Kind", "Currency", "Exchange_Currency")]
public partial class HRMS_Sal_Currency_Rate
{
    /// <summary>
    /// 匯率年月
    /// </summary>
    [Key]
    [Column(TypeName = "date")]
    public DateTime Rate_Month { get; set; }

    /// <summary>
    /// 類別_序號51
    /// </summary>
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string Kind { get; set; }

    /// <summary>
    /// 幣別_序號47
    /// </summary>
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string Currency { get; set; }

    /// <summary>
    /// 兌換幣別
    /// </summary>
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string Exchange_Currency { get; set; }

    /// <summary>
    /// 匯率
    /// </summary>
    [Column(TypeName = "decimal(11, 6)")]
    public decimal Rate { get; set; }

    /// <summary>
    /// 匯率日期
    /// </summary>
    [Column(TypeName = "date")]
    public DateTime Rate_Date { get; set; }

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
