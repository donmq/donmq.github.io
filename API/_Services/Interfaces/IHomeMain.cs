
using API.DTO;
using API.Helper.Attributes;
using SD3_API.Helpers.Utilities;

namespace API._Services.Interfaces
{
    [DependencyInjectionAttribute(ServiceLifetime.Scoped)]
    public interface IHomeMain
    {
        Task<HomeMainDto> GetData(HomeMainParam param);
        Task<List<KeyValuePair<string, string>>> GetListPlayers();
        Task<List<KeyValuePair<int, string>>> GetListExercise();
        Task<List<KeyValuePair<string, string>>> GetListThuocTinh(int ExerciseID, string Position);
        Task<List<KeyValuePair<string, string>>> GetListDisable(string Position);
        Task<OperationResult> Create(DataCreate data);
        Task<OperationResult> Update(DataCreate data);
        Task<OperationResult> Delete(int id);
        Task<List<KeyValuePair<string, string>>> GetKeys();
        Task<List<ListCompares>> GetListCompares(int inforID);
        Task<OperationResult> CreateCompare(ListCompares data);
        Task<OperationResult> DeleteCompare(ListCompares data);

    }
}