using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

[PrimaryKey("USER_GUID", "Seq")]
public partial class HRMS_Emp_Rehire_Evaluation
{
    /// <summary>
    /// 員工GUID
    /// </summary>
    [Key]
    [StringLength(50)]
    public string USER_GUID { get; set; }

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
    /// 序號
    /// </summary>
    [Key]
    public int Seq { get; set; }

    /// <summary>
    /// 評估結果
    /// </summary>
    public bool Results { get; set; }

    /// <summary>
    /// 評估說明
    /// </summary>
    [Required]
    [StringLength(255)]
    public string Explanation { get; set; }

    /// <summary>
    /// 任用事業部
    /// </summary>
    [StringLength(10)]
    [Unicode(false)]
    public string Division { get; set; }

    /// <summary>
    /// 任用廠別
    /// </summary>
    [StringLength(10)]
    [Unicode(false)]
    public string Factory { get; set; }

    /// <summary>
    /// 任用工號
    /// </summary>
    [StringLength(16)]
    [Unicode(false)]
    public string Employee_ID { get; set; }

    /// <summary>
    /// 任用部門
    /// </summary>
    [StringLength(10)]
    [Unicode(false)]
    public string Department { get; set; }

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
