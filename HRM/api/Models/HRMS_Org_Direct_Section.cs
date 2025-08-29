using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

[PrimaryKey("Division", "Factory", "Effective_Date", "Work_Type_Code")]
public partial class HRMS_Org_Direct_Section
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
    /// 生效年月：格式 YYYY/MM
    /// </summary>
    [Key]
    [StringLength(7)]
    [Unicode(false)]
    public string Effective_Date { get; set; }

    /// <summary>
    /// 工種/職務代碼
    /// </summary>
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string Work_Type_Code { get; set; }

    /// <summary>
    /// 工段代碼
    /// </summary>
    [Required]
    [StringLength(10)]
    [Unicode(false)]
    public string Section_Code { get; set; }

    /// <summary>
    /// 是否為直接工段：Y 直接工段；N 間接工段。
    /// </summary>
    [Required]
    [StringLength(1)]
    [Unicode(false)]
    public string Direct_Section { get; set; }

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
