using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

[PrimaryKey("USER_GUID", "Seq")]
public partial class HRMS_Emp_Emergency_Contact
{
    /// <summary>
    /// 員工GUID
    /// </summary>
    [Key]
    [StringLength(50)]
    public string USER_GUID { get; set; }

    /// <summary>
    /// 序號
    /// </summary>
    [Key]
    public int Seq { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string Division { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string Factory { get; set; }

    [StringLength(16)]
    [Unicode(false)]
    public string Employee_ID { get; set; }

    /// <summary>
    /// 緊急連絡者
    /// </summary>
    [Required]
    [StringLength(100)]
    public string Emergency_Contact { get; set; }

    /// <summary>
    /// 親屬關係
    /// </summary>
    [Required]
    [StringLength(10)]
    [Unicode(false)]
    public string Relationship { get; set; }

    /// <summary>
    /// 緊急連絡電話
    /// </summary>
    [Required]
    [StringLength(30)]
    [Unicode(false)]
    public string Emergency_Contact_Phone { get; set; }

    /// <summary>
    /// 暫住地址
    /// </summary>
    [StringLength(255)]
    public string Temporary_Address { get; set; }

    /// <summary>
    /// 緊急連絡地址
    /// </summary>
    [Required]
    [StringLength(255)]
    public string Emergency_Contact_Address { get; set; }

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
