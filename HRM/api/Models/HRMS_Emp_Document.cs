using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

[PrimaryKey("Division", "Factory", "Employee_ID", "Document_Type", "Seq")]
public partial class HRMS_Emp_Document
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
    /// 工號
    /// </summary>
    [Key]
    [StringLength(16)]
    [Unicode(false)]
    public string Employee_ID { get; set; }

    /// <summary>
    /// 證件類別
    /// </summary>
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string Document_Type { get; set; }

    /// <summary>
    /// 序號
    /// </summary>
    [Key]
    public int Seq { get; set; }

    /// <summary>
    /// 護照姓名
    /// </summary>
    [StringLength(100)]
    public string Passport_Name { get; set; }

    /// <summary>
    /// 證件號碼
    /// </summary>
    [Required]
    [StringLength(50)]
    [Unicode(false)]
    public string Document_Number { get; set; }

    /// <summary>
    /// 有效日期起
    /// </summary>
    [Column(TypeName = "datetime")]
    public DateTime Validity_Start { get; set; }

    /// <summary>
    /// 有效日期迄
    /// </summary>
    [Column(TypeName = "datetime")]
    public DateTime Validity_End { get; set; }

    /// <summary>
    /// 程式來源
    /// </summary>
    [StringLength(20)]
    [Unicode(false)]
    public string Program_Code { get; set; }

    /// <summary>
    /// 檔案目錄位置
    /// </summary>
    [StringLength(20)]
    [Unicode(false)]
    public string SerNum { get; set; }

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
