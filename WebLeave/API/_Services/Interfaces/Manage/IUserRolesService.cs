using API.Dtos.Auth;
using API.Dtos.Manage.UserManage;
namespace API._Services.Interfaces.Manage
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface IUserRolesService
    {
        Task<(List<TreeNode<RoleNode>> Roles, List<TreeNode<RoleNode>> AssignedRoles)> GetAllRoleUser(int userId, string langId);
        Task<List<ExportAssignRolesDto>> GetAssignRoles(int userId, string langId);
        Task<OperationResult> AssignRole(int userId, int roleId, int updateBy);
        Task<OperationResult> UnAssignRole(int userId, int roleId);
        Task<OperationResult> UpdateRoleRank(int userId, int roleRank, bool isInherit);
        Task<(List<TreeNode<GroupBaseNode>> Roles, List<TreeNode<GroupBaseNode>> AssignedRoles)> GetAssignGroupBase(int userId, string langId);
        Task<OperationResult> AssignGroupBase(int gbid, int userid);
        Task<OperationResult> UnAssignGroupBase(int gbid, int userid);
        Task<OperationResult> SetPermit(int userId, string key, int updateBy);
        Task<OperationResult> RemovePermit(int userId, string key);
        Task<OperationResult> SetReport(int userId, string key, int updateBy);
        Task<OperationResult> RemoveReport(int userId, string key);
        Task<OperationResult> DownloadExcel(int userId, string langId);
        Task<List<string>> ListUsers(int roleID);
    }
}