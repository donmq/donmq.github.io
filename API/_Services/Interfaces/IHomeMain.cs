
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
        Task<List<KeyValuePair<string, string>>> GetExercisesForAttributes(string Key);
        Task<List<KeyValuePair<string, string>>> GetListDisable(string Position);
        Task<OperationResult> Create(DataCreate data);
        Task<OperationResult> Update(DataCreate data);
        Task<OperationResult> Delete(int id);
        Task<List<KeyValuePair<string, string>>> GetKeys();
        Task<List<Quality>> GetListCompares(int inforID);
        Task<OperationResult> CreateCompare(DataCreate data);
        Task<OperationResult> DeleteCompare(Quality data);

    }
}