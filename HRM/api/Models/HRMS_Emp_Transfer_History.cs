using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

public partial class HRMS_Emp_Transfer_History
{
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
    /// 異動原因
    /// </summary>
    [Required]
    [StringLength(10)]
    [Unicode(false)]
    public string Reason_for_Change { get; set; }

    /// <summary>
    /// 生效日期
    /// </summary>
    [Column(TypeName = "date")]
    public DateTime Effective_Date { get; set; }

    public int? Seq { get; set; }

    /// <summary>
    /// 生效狀態
    /// </summary>
    public bool Effective_Status { get; set; }

    [Required]
    [StringLength(10)]
    [Unicode(false)]
    public string Data_Source { get; set; }

    /// <summary>
    /// 國籍
    /// </summary>
    [Required]
    [StringLength(10)]
    [Unicode(false)]
    public string Nationality_Before { get; set; }

    /// <summary>
    /// 身分證號
    /// </summary>
    [Required]
    [StringLength(50)]
    public string Identification_Number_Before { get; set; }

    /// <summary>
    /// 事業部
    /// </summary>
    [Required]
    [StringLength(10)]
    [Unicode(false)]
    public string Division_Before { get; set; }

    /// <summary>
    /// 廠別
    /// </summary>
    [Required]
    [StringLength(10)]
    [Unicode(false)]
    public string Factory_Before { get; set; }

    /// <summary>
    /// 工號
    /// </summary>
    [Required]
    [StringLength(16)]
    [Unicode(false)]
    public string Employee_ID_Before { get; set; }

    /// <summary>
    /// 部門
    /// </summary>
    [Required]
    [StringLength(10)]
    [Unicode(false)]
    public string Department_Before { get; set; }

    /// <summary>
    /// 派駐/支援事業部
    /// </summary>
    [StringLength(10)]
    [Unicode(false)]
    public string Assigned_Division_Before { get; set; }

    /// <summary>
    /// 派駐/支援廠別
    /// </summary>
    [StringLength(10)]
    [Unicode(false)]
    public string Assigned_Factory_Before { get; set; }

    /// <summary>
    /// 派駐/支援工號
    /// </summary>
    [StringLength(16)]
    [Unicode(false)]
    public string Assigned_Employee_ID_Before { get; set; }

    /// <summary>
    /// 派駐/支援部門
    /// </summary>
    [StringLength(10)]
    [Unicode(false)]
    public string Assigned_Department_Before { get; set; }

    /// <summary>
    /// 職等
    /// </summary>
    [Column(TypeName = "decimal(4, 1)")]
    public decimal Position_Grade_Before { get; set; }

    /// <summary>
    /// 職稱
    /// </summary>
    [Required]
    [StringLength(10)]
    [Unicode(false)]
    public string Position_Title_Before { get; set; }

    /// <summary>
    /// 工種/職務
    /// </summary>
    [Required]
    [StringLength(10)]
    [Unicode(false)]
    public string Work_Type_Before { get; set; }

    [Column(TypeName = "date")]
    public DateTime? ActingPosition_Star_Before { get; set; }

    [Column(TypeName = "date")]
    public DateTime? ActingPosition_End_Before { get; set; }

    /// <summary>
    /// 國籍_異動後
    /// </summary>
    [Required]
    [StringLength(10)]
    [Unicode(false)]
    public string Nationality_After { get; set; }

    /// <summary>
    /// 身分證號_異動後
    /// </summary>
    [Required]
    [StringLength(50)]
    public string Identification_Number_After { get; set; }

    /// <summary>
    /// 事業部_異動後
    /// </summary>
    [Required]
    [StringLength(10)]
    [Unicode(false)]
    public string Division_After { get; set; }

    /// <summary>
    /// 廠別_異動後
    /// </summary>
    [Required]
    [StringLength(10)]
    [Unicode(false)]
    public string Factory_After { get; set; }

    /// <summary>
    /// 工號_異動後
    /// </summary>
    [Required]
    [StringLength(16)]
    [Unicode(false)]
    public string Employee_ID_After { get; set; }

    /// <summary>
    /// 部門_異動後
    /// </summary>
    [Required]
    [StringLength(10)]
    [Unicode(false)]
    public string Department_After { get; set; }

    /// <summary>
    /// 派駐/支援事業部_異動後
    /// </summary>
    [StringLength(10)]
    [Unicode(false)]
    public string Assigned_Division_After { get; set; }

    /// <summary>
    /// 派駐/支援廠別_異動後
    /// </summary>
    [StringLength(10)]
    [Unicode(false)]
    public string Assigned_Factory_After { get; set; }

    /// <summary>
    /// 派駐/支援工號_異動後
    /// </summary>
    [StringLength(16)]
    [Unicode(false)]
    public string Assigned_Employee_ID_After { get; set; }

    /// <summary>
    /// 派駐/支援部門_異動後
    /// </summary>
    [StringLength(10)]
    [Unicode(false)]
    public string Assigned_Department_After { get; set; }

    /// <summary>
    /// 職等_異動後
    /// </summary>
    [Column(TypeName = "decimal(4, 1)")]
    public decimal Position_Grade_After { get; set; }

    /// <summary>
    /// 職稱_異動後
    /// </summary>
    [Required]
    [StringLength(10)]
    [Unicode(false)]
    public string Position_Title_After { get; set; }

    /// <summary>
    /// 工種/職務_異動後
    /// </summary>
    [Required]
    [StringLength(10)]
    [Unicode(false)]
    public string Work_Type_After { get; set; }

    [Column(TypeName = "date")]
    public DateTime? ActingPosition_Star_After { get; set; }

    [Column(TypeName = "date")]
    public DateTime? ActingPosition_End_After { get; set; }

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
