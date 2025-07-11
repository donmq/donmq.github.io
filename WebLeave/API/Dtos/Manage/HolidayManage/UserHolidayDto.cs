namespace API.Dtos.Manage.HolidayManage
{
    public class UserHolidayDto
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string HashPass { get; set; }
        public string HashImage { get; set; }
        public string EmailAddress { get; set; }
        public int? UserRank { get; set; }
        public bool? ISPermitted { get; set; }
        public int? EmpID { get; set; }
        public bool? Visible { get; set; }
        public DateTime? Updated { get; set; }
        public string FullName { get; set; }
    }
}