using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

[PrimaryKey("Division", "Factory", "Contract_Type")]
public partial class HRMS_Emp_Contract_Type
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
    /// 合同名稱
    /// </summary>
    [Required]
    [StringLength(50)]
    public string Contract_Title { get; set; }

    /// <summary>
    /// 試用期
    /// </summary>
    public bool Probationary_Period { get; set; }

    /// <summary>
    /// 合同/試用期年數
    /// </summary>
    public short? Probationary_Year { get; set; }

    /// <summary>
    /// 合同/試用期月數
    /// </summary>
    public short? Probationary_Month { get; set; }

    /// <summary>
    /// 合同/試用期天數
    /// </summary>
    public short? Probationary_Day { get; set; }

    /// <summary>
    /// 預警
    /// </summary>
    public bool Alert { get; set; }

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
