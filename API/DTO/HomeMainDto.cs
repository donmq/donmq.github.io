
using API.Models;

namespace API.DTO
{
    public class HomeMainDto
    {
        public int ID { get; set; }
        public string Ten { get; set; }
        public string ViTri { get; set; }
        public string TuChat { get; set; }
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
        public List<ChatLuongBefore> ChatLuongBefore { get; set; }
    }

    public class DataCreate
    {
        public ChuyenThongTin DataTable { get; set; }
        public ChatLuongBefore[] DataBefore { get; set; }
    }
    public class HomeMainParam
    {
        public string Ten { get; set; }
        public string BaiTap { get; set; }
    }

    public class ChuyenThongTin
    {
        public int ID { get; set; }
        public string Ten { get; set; }
        public string ViTri { get; set; }
        public int DiemTB { get; set; }
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
}