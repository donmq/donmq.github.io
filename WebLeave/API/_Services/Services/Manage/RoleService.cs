using API._Repositories;
using API._Services.Interfaces.Manage;
using API.Models;

namespace API._Services.Services.Manage
{
    public class RoleService : IRoleService
    {
        private readonly IRepositoryAccessor _repoAccessor;
        public RoleService(
            IRepositoryAccessor repoAccessor)
        {
            _repoAccessor = repoAccessor;
        }

        public async Task<Roles> RoleBySym(string Symbol){
            Roles role = await _repoAccessor.Roles.FirstOrDefaultAsync(x => x.RoleSym == Symbol);
            return role;
        }
         /// <summary>
        /// Kiểm tra xem 1 role chỉ định đã được gán cho user chỉ định chưa
        /// </summary>
        public async Task<bool> RoleAssigned(int roleId, int userId)
        {
            return await _repoAccessor.RolesUser.AnyAsync(x => x.RoleID == roleId && x.UserID == userId);
        }
    }
}