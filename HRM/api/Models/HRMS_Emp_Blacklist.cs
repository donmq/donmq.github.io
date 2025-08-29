using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

[PrimaryKey("USER_GUID", "Maintenance_Date")]
public partial class HRMS_Emp_Blacklist
{
    /// <summary>
    /// 員工GUID
    /// </summary>
    [Key]
    [StringLength(50)]
    public string USER_GUID { get; set; }

    /// <summary>
    /// 維護日期
    /// </summary>
    [Key]
    [Column(TypeName = "date")]
    public DateTime Maintenance_Date { get; set; }

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
    /// 離職原因
    /// </summary>
    [Required]
    [StringLength(10)]
    [Unicode(false)]
    public string Resign_Reason { get; set; }

    /// <summary>
    /// 說明
    /// </summary>
    [StringLength(255)]
    public string Description { get; set; }

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
