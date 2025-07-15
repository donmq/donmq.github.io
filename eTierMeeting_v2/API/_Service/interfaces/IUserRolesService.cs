using Machine_API.Helpers.Attributes;
using Machine_API.Models.MachineCheckList;

namespace Machine_API._Service.interfaces
{
    [DependencyInjectionAttribute(ServiceLifetime.Scoped)]
    public interface IUserRolesService
    {
        Task<List<UserRoles>> GetRoleByUser(string user);
    }
}