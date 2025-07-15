using Aspose.Cells;
using Machine_API.DTO;
using Machine_API.Helpers.Attributes;
using Machine_API.Helpers.Params;
using Machine_API.Helpers.Utilities;

namespace Machine_API._Service.interfaces
{
    [DependencyInjectionAttribute(ServiceLifetime.Scoped)]
    public interface IUserService
    {
        Task<OperationResult> AddUser(UserDto userDto, string userName, string lang);
        Task<OperationResult> UpdateUser(UserDto userDto, string userName, string lang);
        Task<OperationResult> RestoreUser(string userName, string userNameUpdate, string lang);
        Task<OperationResult> RemoveUser(string userName, string sessionUsername, string lang);
        Task<PageListUtility<UserDto>> SearchUser(PaginationParams paginationParams, string keyword);
        Task<List<UserDto>> GetAllUserPreliminary();
        Task<List<UserExportDto>> GetAllUser();
        Task<UserDto> GetUserName(string userName);
        void PutStaticValue(ref Worksheet ws);
    }
}