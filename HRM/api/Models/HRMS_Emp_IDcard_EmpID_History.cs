using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

public partial class HRMS_Emp_IDcard_EmpID_History
{
    /// <summary>
    /// 歷程GUID
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
    /// 事業部
    /// </summary>
    [Required]
    [StringLength(10)]
    [Unicode(false)]
    public string Division { get; set; }

    /// <summary>
    /// 廠別
    /// </summary>
    [Required]
    [StringLength(10)]
    [Unicode(false)]
    public string Factory { get; set; }

    /// <summary>
    /// 工號
    /// </summary>
    [Required]
    [StringLength(16)]
    [Unicode(false)]
    public string Employee_ID { get; set; }

    /// <summary>
    /// 部門
    /// </summary>
    [StringLength(10)]
    [Unicode(false)]
    public string Department { get; set; }

    /// <summary>
    /// 派駐/支援事業部
    /// </summary>
    [StringLength(10)]
    [Unicode(false)]
    public string Assigned_Division { get; set; }

    /// <summary>
    /// 派駐/支援廠別
    /// </summary>
    [StringLength(10)]
    [Unicode(false)]
    public string Assigned_Factory { get; set; }

    /// <summary>
    /// 派駐/支援工號
    /// </summary>
    [StringLength(16)]
    [Unicode(false)]
    public string Assigned_Employee_ID { get; set; }

    /// <summary>
    /// 派駐/支援部門
    /// </summary>
    [StringLength(10)]
    [Unicode(false)]
    public string Assigned_Department { get; set; }

    /// <summary>
    /// 到職日期
    /// </summary>
    [Column(TypeName = "date")]
    public DateTime Onboard_Date { get; set; }

    /// <summary>
    /// 離職日期
    /// </summary>
    [Column(TypeName = "date")]
    public DateTime? Resign_Date { get; set; }

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
