using API.Models;

namespace API.Dtos.Manage.UserManage
{
    public partial class UserForDetailDto
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string HashPass { get; set; }
        public string HashImage { get; set; }
        public string EmailAddress { get; set; }
        public int? UserRank { get; set; }
        public bool? ISPermitted { get; set; }
        public int? RolePermitted { get; set; }
        public int? RoleReport { get; set; }
        public int? EmpID { get; set; }
        public bool? Visible { get; set; }
        public DateTime? Updated { get; set; }
        public string FullName { get; set; }
        public string EmpNumber { get; set; }
        public string PartName { get; set; }
        public string BaseName { get; set; }
        public Employee Employee { get; set; }
        public UserForDetailDto()
        {
            this.Updated = DateTime.Now;
        }
        public string UserRankString { get; set; }
    }
    public partial class UserManageTitleExcel
    {
        public string label_Username { get; set; }
        public string label_Fullname { get; set; }
        public string label_Email { get; set; }
        public string label_Visible { get; set; }
        public string label_RankTitle { get; set; }
        public string label_CurrentRole { get; set; }
        public string label_Rank1 { get; set; }
        public string label_Rank2 { get; set; }
        public string label_Rank3 { get; set; }
        public string label_Rank6 { get; set; }
        public string label_Rank5 { get; set; }
    }

    public class AreaNode
    {
        public string AreaName { get; set; }
        public List<DepartmentNode> Departments { get; set; }
    }

    public class DepartmentNode
    {
        public string DeptName { get; set; }
        public List<PartNode> Parts { get; set; }
    }

    public class PartNode
    {
        public string PartName { get; set; }
    }
}