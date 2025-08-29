using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

public partial class HRMS_Basic_Account
{
    /// <summary>
    /// 帳號
    /// </summary>
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string Account { get; set; }

    /// <summary>
    /// 密碼
    /// </summary>
    [StringLength(10)]
    [Unicode(false)]
    public string Password { get; set; }

    /// <summary>
    /// 姓名
    /// </summary>
    [StringLength(50)]
    [Unicode(false)]
    public string Name { get; set; }

    /// <summary>
    /// 事業部
    /// </summary>
    [StringLength(10)]
    [Unicode(false)]
    public string Division { get; set; }

    /// <summary>
    /// 廠別
    /// </summary>
    [StringLength(10)]
    [Unicode(false)]
    public string Factory { get; set; }

    /// <summary>
    /// 部門代碼
    /// </summary>
    [StringLength(15)]
    [Unicode(false)]
    public string Department_ID { get; set; }

    /// <summary>
    /// 啟用：1 啟用 0停用
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// 密碼還原 1.還原初始密碼  0.舊密碼
    /// </summary>
    public bool? Password_Reset { get; set; }

    /// <summary>
    /// 異動者
    /// </summary>
    [StringLength(20)]
    [Unicode(false)]
    public string Update_By { get; set; }

    /// <summary>
    /// 異動日
    /// </summary>
    [Column(TypeName = "datetime")]
    public DateTime? Update_Time { get; set; }
}
