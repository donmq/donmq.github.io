using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

[PrimaryKey("Account", "Role")]
public partial class HRMS_Basic_Account_Role
{
    /// <summary>
    /// 帳號
    /// </summary>
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string Account { get; set; }

    /// <summary>
    /// 角色
    /// </summary>
    [Key]
    [StringLength(20)]
    [Unicode(false)]
    public string Role { get; set; }

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
