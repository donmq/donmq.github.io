using Machine_API.Models.MachineCheckList;

namespace Machine_API.DTO
{
    public class UserDto
    {
        public int UserID { get; set; }

        public string UserName { get; set; }

        public string HashPass { get; set; }

        public string HashImage { get; set; }

        public string EmailAddress { get; set; }

        public bool? Visible { get; set; }

        public DateTime? UpdateDate { get; set; }

        public string UpdateBy { get; set; }

        public string EmpName { get; set; }

        public List<int?> Roles { get; set; }

        public List<UserRoles> ListRoles { get; set; }

    }
}