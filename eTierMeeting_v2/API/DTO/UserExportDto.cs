namespace Machine_API.DTO
{
    public class UserExportDto
    {
        public string UserName { get; set; }
        public string EmailAddress { get; set; }
        public string EmpName { get; set; }
        public List<int> Roles { get; set; }
        public string ListRolesName { get; set; }

    }
}