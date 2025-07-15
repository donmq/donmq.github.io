namespace Machine_API.DTO
{
    public class PreliminaryPlnoAddDTO
    {
        public string EmpName { get; set; }
        public string EmpNumber { get; set; }
        public string UpdateBy { get; set; }
        public List<UserRoleAddDto> RoleList { get; set; }
    }
}