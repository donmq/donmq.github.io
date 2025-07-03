using System.Linq;
using System.Threading.Tasks;
using eTierV2_API._Repositories;
using eTierV2_API._Repositories.Interfaces;
using eTierV2_API._Services.Interfaces;
using eTierV2_API.DTO;
using Microsoft.EntityFrameworkCore;

namespace eTierV2_API._Services.Services
{
    public class AuthService : IAuthService
    {
        private readonly IRepositoryAccessor _repoAccessor;
        public AuthService( IRepositoryAccessor repoAccessor)
        {
            _repoAccessor = repoAccessor;
        }

        public async Task<UserForLoggedDTO> GetUser(string username, string password)
        {
            var user = _repoAccessor.Users.FirstOrDefault(x => x.account.Trim() == username.Trim() && x.is_active == true);

            // kiểm tra xem username đó có ko
            if (user == null)
            {
                return null;
            }
            if (user.password != password)
            {
                return null;
            }
            var roleUser = _repoAccessor.RoleUser.FindAll(x => x.user_account == user.account);
            var role = _repoAccessor.Roles.FindAll();
            var roleName = await roleUser.Join(role, x => x.role_unique, y => y.role_unique, (x, y)
            => new RoleDTO { Name = y.role_unique, Position = y.role_sequence }).ToListAsync();

            var result = new UserForLoggedDTO
            {
                Id = user.account,
                Email = user.email,
                Username = user.account,
                Name = user.name,
                // Nik = user.,
                Role = roleName.OrderBy(x => x.Position).Select(x => x.Name).ToList()
            };

            return result;
        }
    }
}