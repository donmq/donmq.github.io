using System.Threading.Tasks;
using eTierV2_API.DTO;

namespace eTierV2_API._Services.Interfaces
{
     public interface IAuthService
    {
         Task<UserForLoggedDTO> GetUser(string username,string password);
    }
}