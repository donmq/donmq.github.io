using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

[PrimaryKey("Division", "Factory", "Employee_ID", "Seq")]
public partial class HRMS_Emp_Certification
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
    /// 序號
    /// </summary>
    [Key]
    public int Seq { get; set; }

    /// <summary>
    /// 證照類別
    /// </summary>
    [Required]
    [StringLength(10)]
    [Unicode(false)]
    public string Certification { get; set; }

    /// <summary>
    /// 證照測驗名稱
    /// </summary>
    [Required]
    [StringLength(50)]
    public string Name_Of_Certification { get; set; }

    /// <summary>
    /// 分數
    /// </summary>
    [StringLength(10)]
    public string Score { get; set; }

    /// <summary>
    /// 級數
    /// </summary>
    [StringLength(10)]
    public string Level { get; set; }

    /// <summary>
    /// 合格狀態
    /// </summary>
    public bool Result { get; set; }

    /// <summary>
    /// 通過日期
    /// </summary>
    [Column(TypeName = "date")]
    public DateTime Passing_Date { get; set; }

    /// <summary>
    /// 有效日期
    /// </summary>
    [Column(TypeName = "date")]
    public DateTime? Certification_Valid_Period { get; set; }

    /// <summary>
    /// 備註
    /// </summary>
    [StringLength(50)]
    public string Remark { get; set; }

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
