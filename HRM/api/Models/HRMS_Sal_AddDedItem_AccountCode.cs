using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

[PrimaryKey("Factory", "AddDed_Item")]
public partial class HRMS_Sal_AddDedItem_AccountCode
{
    /// <summary>
    /// 廠別
    /// </summary>
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string Factory { get; set; }

    /// <summary>
    /// 加扣項目_序號49
    /// </summary>
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string AddDed_Item { get; set; }

    /// <summary>
    /// 主科目
    /// </summary>
    [Required]
    [StringLength(4)]
    [Unicode(false)]
    public string Main_Acc { get; set; }

    /// <summary>
    /// 子科目
    /// </summary>
    [Required]
    [StringLength(4)]
    [Unicode(false)]
    public string Sub_Acc { get; set; }

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
