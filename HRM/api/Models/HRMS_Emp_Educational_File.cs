using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

[PrimaryKey("USER_GUID", "SerNum", "FileID")]
public partial class HRMS_Emp_Educational_File
{
    /// <summary>
    /// 員工GUID
    /// </summary>
    [Key]
    [StringLength(50)]
    public string USER_GUID { get; set; }

    /// <summary>
    /// 檔案目錄位置
    /// </summary>
    [Key]
    [StringLength(20)]
    [Unicode(false)]
    public string SerNum { get; set; }

    /// <summary>
    /// 檔案ID
    /// </summary>
    [Key]
    public int FileID { get; set; }

    /// <summary>
    /// 檔名
    /// </summary>
    [Required]
    public string FileName { get; set; }

    /// <summary>
    /// 檔案大小
    /// </summary>
    public double FileSize { get; set; }

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
