using System.ComponentModel.DataAnnotations;

namespace API.Models;

public partial class ChatLuongCauThu
{
    [Key]
    public int ID { get; set; }
    public int? IDThongTin { get; set; }
    public int? IDThuocTinhChinh { get; set; }
    public int? ChatLuong { get; set; }
}