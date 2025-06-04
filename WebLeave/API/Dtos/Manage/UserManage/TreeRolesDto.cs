
namespace API.Dtos.Manage.UserManage
{
    public partial class TreeRolesDto
    {
        public int id { get; set; }
        public string text { get; set; } //name
        public int value { get; set; } //roleID
        public bool @checked { get; set; }
        public ICollection<TreeRolesDto> children { get; set; }
    }
}