using Machine_API.DTO;
using Machine_API.Helpers.Attributes;
using Machine_API.Helpers.Utilities;

namespace Machine_API._Services.Interface
{
    [DependencyInjectionAttribute(ServiceLifetime.Scoped)]
    public interface IAuthService 
    {
        Task<UserDto> Login(UserForLoginDto model);
        Task<OperationResult> ChangePassword(ChangePasswordDto changePassword, string lang);
    }
}