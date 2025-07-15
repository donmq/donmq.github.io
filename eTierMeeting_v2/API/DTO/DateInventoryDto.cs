

namespace Machine_API.DTO
{
    public class DateInventoryDto
    {
        public int Id { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string Content { get; set; }
        public string EmpName { get; set; }
        public DateTime? CreateTime { get; set; }
        public string CreateBy { get; set; }
    }
}