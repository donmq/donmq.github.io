
using API.DTO;
using API.Helper.Attributes;

namespace API._Services.Interfaces
{
    [DependencyInjectionAttribute(ServiceLifetime.Scoped)]
    public interface IHomeMain
    {
        Task<HomeMainDto> GetData(HomeMainParam param);
        Task<List<KeyValuePair<string, string>>> GetListPlayers();
        Task<List<KeyValuePair<string, string>>> GetListExercise();
    }
}