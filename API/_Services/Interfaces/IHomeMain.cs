
using API.DTO;
using API.Helper.Attributes;

namespace API._Services.Interfaces
{
    [DependencyInjectionAttribute(ServiceLifetime.Scoped)]
    public interface IHomeMain
    {
        Task<HomeMainDto> GetData(HomeMainParam param);
        Task<List<KeyValuePair<string, string>>> GetListPlayers();
        Task<List<KeyValuePair<int, string>>> GetListExercise();
        Task<List<KeyValuePair<string, string>>> GetListThuocTinh(int IDBaiTap, string ViTri);
        Task<List<KeyValuePair<string, string>>> GetListDisable(string ViTri);
    }
}