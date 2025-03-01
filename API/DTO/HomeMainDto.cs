
using API.Models;

namespace API.DTO
{
    public class HomeMainDto
    {
        public int ID { get; set; }
        public int PlanID { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public string Personality { get; set; }
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
        public List<Quality> QualityAfter { get; set; }
    }

    public class DataCreate
    {
        public Quality DataTable { get; set; }
        public Quality[] DataAfter { get; set; }
    }
    public class HomeMainParam
    {
        public int InforID { get; set; }
        public int ExerciseID { get; set; }
        public int PlanID { get; set; }
    }

    public class Quality
    {
        public int ID { get; set; }
        public int InforID { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public int Average { get; set; }
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
        public int ChatLuongChung { get; set; }
        public int PlanID { get; set; }
        public int SoDiemTap { get; set; }
        public int ExerciseID { get; set; }
    }
}