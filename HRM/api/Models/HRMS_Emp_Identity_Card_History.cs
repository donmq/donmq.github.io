using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

public partial class HRMS_Emp_Identity_Card_History
{
    /// <summary>
    /// 身分證歷程GUID
    /// </summary>
    [Key]
    [StringLength(50)]
    public string History_GUID { get; set; }

    /// <summary>
    /// 員工GUID
    /// </summary>
    [Required]
    [StringLength(50)]
    public string USER_GUID { get; set; }

    /// <summary>
    /// 國籍-異動前
    /// </summary>
    [Required]
    [StringLength(10)]
    [Unicode(false)]
    public string Nationality_Before { get; set; }

    /// <summary>
    /// 身分證號-異動前
    /// </summary>
    [Required]
    [StringLength(50)]
    public string Identification_Number_Before { get; set; }

    /// <summary>
    /// 發行日-異動前
    /// </summary>
    [Column(TypeName = "date")]
    public DateTime Issued_Date_Before { get; set; }

    /// <summary>
    /// 國籍-異動後
    /// </summary>
    [Required]
    [StringLength(10)]
    [Unicode(false)]
    public string Nationality_After { get; set; }

    /// <summary>
    /// 身分證號-異動後
    /// </summary>
    [Required]
    [StringLength(50)]
    public string Identification_Number_After { get; set; }

    /// <summary>
    /// 發行日-異動後
    /// </summary>
    [Column(TypeName = "date")]
    public DateTime Issued_Date_After { get; set; }

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
