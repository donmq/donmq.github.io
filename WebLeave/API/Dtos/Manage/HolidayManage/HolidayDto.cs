namespace API.Dtos.Manage.HolidayManage
{
    public class HolidayDto
    {
        public int HolidayID { get; set; }
        public string Date { get; set; }
        public string Description { get; set; }
        public int? UserID { get; set; }
        public DateTime? CreateTime { get; set; }
    }
}