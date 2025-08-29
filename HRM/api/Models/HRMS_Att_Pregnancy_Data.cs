using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

[PrimaryKey("Factory", "Employee_ID", "Due_Date")]
public partial class HRMS_Att_Pregnancy_Data
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

    [StringLength(10)]
    [Unicode(false)]
    public string Department { get; set; }

    /// <summary>
    /// 懷孕前工種
    /// </summary>
    [StringLength(10)]
    [Unicode(false)]
    public string Work_Type_Before { get; set; }

    /// <summary>
    /// 懷孕後工種
    /// </summary>
    [StringLength(10)]
    [Unicode(false)]
    public string Work_Type_After { get; set; }

    /// <summary>
    /// 預產日期
    /// </summary>
    [Key]
    [Column(TypeName = "date")]
    public DateTime Due_Date { get; set; }

    /// <summary>
    /// 懷孕週數
    /// </summary>
    [Column(TypeName = "decimal(3, 1)")]
    public decimal Pregnancy_Week { get; set; }

    /// <summary>
    /// 照超音波期
    /// </summary>
    [Column(TypeName = "date")]
    public DateTime? Ultrasound_Date { get; set; }

    /// <summary>
    /// 開始上班7小時日期
    /// </summary>
    [Column(TypeName = "date")]
    public DateTime? Work7hours { get; set; }

    /// <summary>
    /// 懷孕滿36週日期
    /// </summary>
    [Column(TypeName = "date")]
    public DateTime? Pregnancy36Weeks { get; set; }

    /// <summary>
    /// 產假開始日期
    /// </summary>
    [Column(TypeName = "date")]
    public DateTime? Maternity_Start { get; set; }

    /// <summary>
    /// 產假結束日期
    /// </summary>
    [Column(TypeName = "date")]
    public DateTime? Maternity_End { get; set; }

    /// <summary>
    /// 回廠日期
    /// </summary>
    [Column(TypeName = "date")]
    public DateTime? GoWork_Date { get; set; }

    /// <summary>
    /// 結案
    /// </summary>
    public bool? Close_Case { get; set; }

    /// <summary>
    /// 備註
    /// </summary>
    [StringLength(100)]
    public string Remark { get; set; }

    /// <summary>
    /// 育嬰開始日期
    /// </summary>
    [Column(TypeName = "date")]
    public DateTime? Baby_Start { get; set; }

    /// <summary>
    /// 育嬰結束日期
    /// </summary>
    [Column(TypeName = "date")]
    public DateTime? Baby_End { get; set; }

    /// <summary>
    /// 預計產檢日期1
    /// </summary>
    [Column(TypeName = "date")]
    public DateTime? Estimated_Date1 { get; set; }

    /// <summary>
    /// 預計產檢日期2
    /// </summary>
    [Column(TypeName = "date")]
    public DateTime? Estimated_Date2 { get; set; }

    /// <summary>
    /// 預計產檢日期3
    /// </summary>
    [Column(TypeName = "date")]
    public DateTime? Estimated_Date3 { get; set; }

    /// <summary>
    /// 預計產檢日期4
    /// </summary>
    [Column(TypeName = "date")]
    public DateTime? Estimated_Date4 { get; set; }

    /// <summary>
    /// 預計產檢日期5
    /// </summary>
    [Column(TypeName = "date")]
    public DateTime? Estimated_Date5 { get; set; }

    /// <summary>
    /// 保險產檢日期1
    /// </summary>
    [Column(TypeName = "date")]
    public DateTime? Insurance_Date1 { get; set; }

    /// <summary>
    /// 保險產檢日期2
    /// </summary>
    [Column(TypeName = "date")]
    public DateTime? Insurance_Date2 { get; set; }

    /// <summary>
    /// 保險產檢日期3
    /// </summary>
    [Column(TypeName = "date")]
    public DateTime? Insurance_Date3 { get; set; }

    /// <summary>
    /// 保險產檢日期4
    /// </summary>
    [Column(TypeName = "date")]
    public DateTime? Insurance_Date4 { get; set; }

    /// <summary>
    /// 保險產檢日期5
    /// </summary>
    [Column(TypeName = "date")]
    public DateTime? Insurance_Date5 { get; set; }

    /// <summary>
    /// 請假產檢日期1
    /// </summary>
    [Column(TypeName = "date")]
    public DateTime? Leave_Date1 { get; set; }

    /// <summary>
    /// 請假產檢日期2
    /// </summary>
    [Column(TypeName = "date")]
    public DateTime? Leave_Date2 { get; set; }

    /// <summary>
    /// 請假產檢日期3
    /// </summary>
    [Column(TypeName = "date")]
    public DateTime? Leave_Date3 { get; set; }

    /// <summary>
    /// 請假產檢日期4
    /// </summary>
    [Column(TypeName = "date")]
    public DateTime? Leave_Date4 { get; set; }

    /// <summary>
    /// 請假產檢日期5
    /// </summary>
    [Column(TypeName = "date")]
    public DateTime? Leave_Date5 { get; set; }

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
