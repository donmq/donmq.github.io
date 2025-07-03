using System.Collections.Generic;
using System.Threading.Tasks;
using eTierV2_API.DTO;
using eTierV2_API.Helpers.Params;
using eTierV2_API.Helpers.Utilities;

namespace eTierV2_API._Services.Interfaces
{
    public interface IUserService
    {
        Task<PagedList<UserDTO>> GetListUserPaging(string account, string isActive, int pageNumber = 10, int pageSize = 10);
        Task<bool> AddUser(UserDTO user, string updateBy);
        Task<bool> UpdateUser(UserDTO user, string updateBy);
        Task<List<RoleByUserDTO>> GetRoleByUser(string account);
        Task<bool> UpdateRoleByUser(string account, List<RoleByUserDTO> roles, string updateBy);
        Task<bool> CheckExistUser(string account);
        Task<OperationResult> ChangePassword(UserForLoginDto user);
    }
}