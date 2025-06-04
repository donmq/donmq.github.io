using API.Dtos.SeaHr.ViewModel;
using API.Helpers.Params.Seahr;
namespace API._Services.Services.SeaHr
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface IAddManuallyService
    {
        Task<OperationResult> DeleteManual(string leaveId);
        Task<OperationResult> AddManually(AddManuallyParam param, string userId);
        Task<AddManuallyViewModel> GetDetail(int leaveId);
        Task<List<KeyValuePair<int,string>>> GetAllCategory(string lang);
        Task<double?> GetCountRestAgent(int year, string empNumber);
        Task<string> CheckDateLeave(string start, string end, string empNumber);
        Task<bool> CheckIsSun(string empNumber);
    }
}