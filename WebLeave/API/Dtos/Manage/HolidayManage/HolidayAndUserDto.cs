namespace API.Dtos.Manage.HolidayManage
{
    public class HolidayAndUserDto
    {
        public int HolidayID { get; set; }
        public DateTime? Date { get; set; }
        public string Description { get; set; }
        public int? UserID { get; set; }
        public DateTime? CreateTime { get; set; }
        public string FullName { get; set; }
    }
}