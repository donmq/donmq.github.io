using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

public partial class HRMS_Emp_Personal
{
    /// <summary>
    /// 員工GUID
    /// </summary>
    [Key]
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
    /// 身分證發行日
    /// </summary>
    [Column(TypeName = "date")]
    public DateTime Issued_Date { get; set; }

    /// <summary>
    /// 公司
    /// </summary>
    [Required]
    [StringLength(10)]
    [Unicode(false)]
    public string Company { get; set; }

    /// <summary>
    /// 人員狀態Y.在職 N.離職
    /// </summary>
    [Required]
    [StringLength(1)]
    [Unicode(false)]
    public string Deletion_Code { get; set; }

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
    [Required]
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
    /// 權限身分別
    /// </summary>
    [Required]
    [StringLength(10)]
    [Unicode(false)]
    public string Permission_Group { get; set; }

    /// <summary>
    /// 人員狀態
    /// </summary>
    [StringLength(5)]
    [Unicode(false)]
    public string Employment_Status { get; set; }

    /// <summary>
    /// 考核權責事業部
    /// </summary>
    [StringLength(10)]
    [Unicode(false)]
    public string Performance_Division { get; set; }

    /// <summary>
    /// 身分別
    /// </summary>
    [Required]
    [StringLength(10)]
    [Unicode(false)]
    public string Identity_Type { get; set; }

    /// <summary>
    /// 本地姓名
    /// </summary>
    [Required]
    [StringLength(100)]
    public string Local_Full_Name { get; set; }

    /// <summary>
    /// 慣用英文姓名
    /// </summary>
    [StringLength(100)]
    public string Preferred_English_Full_Name { get; set; }

    /// <summary>
    /// 中文姓名
    /// </summary>
    [StringLength(50)]
    public string Chinese_Name { get; set; }

    /// <summary>
    /// 性別
    /// </summary>
    [Required]
    [StringLength(1)]
    [Unicode(false)]
    public string Gender { get; set; }

    /// <summary>
    /// 血型
    /// </summary>
    [StringLength(1)]
    [Unicode(false)]
    public string Blood_Type { get; set; }

    /// <summary>
    /// 婚姻狀況
    /// </summary>
    [Required]
    [StringLength(1)]
    [Unicode(false)]
    public string Marital_Status { get; set; }

    /// <summary>
    /// 出生日期
    /// </summary>
    [Column(TypeName = "date")]
    public DateTime Birthday { get; set; }

    /// <summary>
    /// 電話
    /// </summary>
    [Required]
    [StringLength(30)]
    [Unicode(false)]
    public string Phone_Number { get; set; }

    /// <summary>
    /// 手機號碼
    /// </summary>
    [StringLength(30)]
    [Unicode(false)]
    public string Mobile_Phone_Number { get; set; }

    /// <summary>
    /// 學歷
    /// </summary>
    [Required]
    [StringLength(10)]
    [Unicode(false)]
    public string Education { get; set; }

    /// <summary>
    /// 宗教
    /// </summary>
    [StringLength(10)]
    [Unicode(false)]
    public string Religion { get; set; }

    /// <summary>
    /// 交通方式
    /// </summary>
    [StringLength(10)]
    [Unicode(false)]
    public string Transportation_Method { get; set; }

    /// <summary>
    /// 車種
    /// </summary>
    [StringLength(30)]
    [Unicode(false)]
    public string Vehicle_Type { get; set; }

    /// <summary>
    /// 車號
    /// </summary>
    [StringLength(20)]
    [Unicode(false)]
    public string License_Plate_Number { get; set; }

    /// <summary>
    /// 戶籍_省/直轄市/縣
    /// </summary>
    [Required]
    [StringLength(10)]
    [Unicode(false)]
    public string Registered_Province_Directly { get; set; }

    /// <summary>
    /// 戶籍_市/區/縣
    /// </summary>
    [Required]
    [StringLength(10)]
    [Unicode(false)]
    public string Registered_City { get; set; }

    /// <summary>
    /// 戶籍地址
    /// </summary>
    [Required]
    [StringLength(255)]
    public string Registered_Address { get; set; }

    /// <summary>
    /// 通訊_省/直轄市/縣
    /// </summary>
    [StringLength(10)]
    [Unicode(false)]
    public string Mailing_Province_Directly { get; set; }

    /// <summary>
    /// 通訊_市/區/縣
    /// </summary>
    [StringLength(10)]
    [Unicode(false)]
    public string Mailing_City { get; set; }

    /// <summary>
    /// 通訊地址
    /// </summary>
    [StringLength(255)]
    public string Mailing_Address { get; set; }

    /// <summary>
    /// 班別
    /// </summary>
    [Required]
    [StringLength(10)]
    [Unicode(false)]
    public string Work_Shift_Type { get; set; }

    /// <summary>
    /// 刷卡否
    /// </summary>
    public bool Swipe_Card_Option { get; set; }

    /// <summary>
    /// 刷卡號
    /// </summary>
    [StringLength(20)]
    public string Swipe_Card_Number { get; set; }

    /// <summary>
    /// 職等
    /// </summary>
    [Column(TypeName = "decimal(4, 1)")]
    public decimal Position_Grade { get; set; }

    /// <summary>
    /// 職稱
    /// </summary>
    [Required]
    [StringLength(10)]
    [Unicode(false)]
    public string Position_Title { get; set; }

    /// <summary>
    /// 工種/職務
    /// </summary>
    [Required]
    [StringLength(10)]
    [Unicode(false)]
    public string Work_Type { get; set; }

    /// <summary>
    /// 餐廳
    /// </summary>
    [StringLength(10)]
    [Unicode(false)]
    public string Restaurant { get; set; }

    /// <summary>
    /// 工作地點
    /// </summary>
    [StringLength(10)]
    [Unicode(false)]
    public string Work_Location { get; set; }

    /// <summary>
    /// 加入工會
    /// </summary>
    public bool? Union_Membership { get; set; }

    /// <summary>
    /// 懷孕上班8小時
    /// </summary>
    public bool? Work8hours { get; set; }

    /// <summary>
    /// 到職日期
    /// </summary>
    [Column(TypeName = "date")]
    public DateTime Onboard_Date { get; set; }

    /// <summary>
    /// 集團到職日
    /// </summary>
    [Column(TypeName = "date")]
    public DateTime Group_Date { get; set; }

    /// <summary>
    /// 年資起算日
    /// </summary>
    [Column(TypeName = "date")]
    public DateTime Seniority_Start_Date { get; set; }

    /// <summary>
    /// 年假年資起算日
    /// </summary>
    [Column(TypeName = "date")]
    public DateTime Annual_Leave_Seniority_Start_Date { get; set; }

    /// <summary>
    /// 離職日期
    /// </summary>
    [Column(TypeName = "date")]
    public DateTime? Resign_Date { get; set; }

    /// <summary>
    /// 離職原因
    /// </summary>
    [StringLength(10)]
    [Unicode(false)]
    public string Resign_Reason { get; set; }

    /// <summary>
    /// 黑名單
    /// </summary>
    public bool? Blacklist { get; set; }

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
