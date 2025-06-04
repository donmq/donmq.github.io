using API.Dtos.Auth;
using API.Helpers.Params;
namespace API._Services.Interfaces
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface IAuthService
    {
        Task<UserForLoggedDto> Login(UserForLoginParam userForLogin, UsersDto usersDto);
        Task<int> CountLeaveEdit(int userID);
        Task<int> CountSeaHrEdit();
        Task<int> CountSeaHrConfirm();
        Task<OperationResult> ChangePassword(UserForLoginParam userForLogin);
        Task<bool> CheckLoggedIn(UserForLoginParam userForLogin);
        Task Logout(UserForLoginParam userForLogin);
        Task<UsersDto> GetUser(UserForLoginParam userForLogin);
    }
}