
namespace API.Dtos.Manage.UserManage
{
    public partial class ExportAssignRolesDto
    {
        public string Number { get; set; }
        public string FullName { get; set; }
        public string BuildingName { get; set; }
        public string DeptMainName { get; set; }
        public string DeptName { get; set; }
        public string Employees { get; set; }
    }

    public partial class ExportUserRolesDto
    {
        public string BuildingName { get; set; }
        public string DeptMainName { get; set; }
        public string DeptName { get; set; }
    }
}