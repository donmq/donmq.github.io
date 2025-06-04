using API.Dtos.Leave;
using API.Helpers.Utilities;

namespace API._Services.Interfaces.Leave
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface ILeaveSurrogateService
    {
        Task<List<KeyValueUtility>> GetSurrogates(int userId);
        Task<SurrogateDto> GetDetail(int userId);
        Task<OperationResult> SaveSurrogate(SurrogateDto dto);
        Task<OperationResult> RemoveSurrogate(SurrogateRemoveDto dto);
    }
}