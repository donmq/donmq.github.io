using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

public partial class ChatLuongBefore
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("IDThongTin")]
    public int? IdthongTin { get; set; }

    [Column("IDBaiTap")]
    public int? IdbaiTap { get; set; }

    [Column("DiemTB")]
    public int? DiemTb { get; set; }

    public int? CanPha { get; set; }

    public int? KemNguoi { get; set; }

    public int? ChayCho { get; set; }

    public int? DanhDau { get; set; }

    public int? DungCam { get; set; }

    public int? ChuyenBong { get; set; }

    public int? ReBong { get; set; }

    public int? TatCanh { get; set; }

    public int? SutManh { get; set; }

    public int? DutDiem { get; set; }

    public int? TheLuc { get; set; }

    public int? SucManh { get; set; }

    public int? XongXao { get; set; }

    public int? TocDo { get; set; }

    public int? SangTao { get; set; }
}
