using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

public partial class HRMS_Rew_EmpRecords
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
    /// 獎懲日期
    /// </summary>
    [Column(TypeName = "date")]
    public DateTime Reward_Date { get; set; }

    /// <summary>
    /// 獎懲類別_序號66
    /// </summary>
    [Required]
    [StringLength(10)]
    [Unicode(false)]
    public string Reward_Type { get; set; }

    /// <summary>
    /// 原因代碼
    /// </summary>
    [StringLength(10)]
    [Unicode(false)]
    public string Reason_Code { get; set; }

    /// <summary>
    /// 薪資年月
    /// </summary>
    [Column(TypeName = "date")]
    public DateTime? Sal_Month { get; set; }

    /// <summary>
    /// 次數
    /// </summary>
    public short Reward_Times { get; set; }

    /// <summary>
    /// 備註
    /// </summary>
    [StringLength(100)]
    public string Remark { get; set; }

    /// <summary>
    /// 程式來源
    /// </summary>
    [StringLength(20)]
    [Unicode(false)]
    public string Program_Code { get; set; }

    /// <summary>
    /// 檔案目錄位置
    /// </summary>
    [StringLength(20)]
    [Unicode(false)]
    public string SerNum { get; set; }

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
