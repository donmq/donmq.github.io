using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

[PrimaryKey("Factory", "Employee_ID", "Birthday_Child")]
public partial class HRMS_Sal_Childcare_Subsidy
{
    /// <summary>
    /// 員工GUID
    /// </summary>
    [Required]
    [StringLength(50)]
    public string USER_GUID { get; set; }

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
    /// 小孩出生日期
    /// </summary>
    [Key]
    [Column(TypeName = "date")]
    public DateTime Birthday_Child { get; set; }

    /// <summary>
    /// 開始年月
    /// </summary>
    [Column(TypeName = "date")]
    public DateTime Month_Start { get; set; }

    /// <summary>
    /// 結束年月
    /// </summary>
    [Column(TypeName = "date")]
    public DateTime Month_End { get; set; }

    /// <summary>
    /// 人數
    /// </summary>
    public short Num_Children { get; set; }

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
