﻿using System.ComponentModel.DataAnnotations;

namespace API.Models;

public partial class ChatLuongBefore
{
    [Key]
    public int ID { get; set; }
    public int? IDThongTin { get; set; }
    public int? IDBaiTap { get; set; }
    public int? DiemTB { get; set; }
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