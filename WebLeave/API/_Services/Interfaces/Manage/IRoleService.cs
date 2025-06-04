using API.Models;
namespace API._Services.Interfaces.Manage
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface IRoleService
    {
        Task<Roles> RoleBySym(string Symbol);
        Task<bool> RoleAssigned(int roleId, int userId);
    }
}