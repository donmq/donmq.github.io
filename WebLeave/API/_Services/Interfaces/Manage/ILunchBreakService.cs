using API.Dtos.Common;

namespace API._Services.Interfaces.Manage
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface ILunchBreakService
    {
        Task<OperationResult> Create(LunchBreakDto PositionAndPosLang);
        Task<OperationResult> Update(LunchBreakDto PositionAndPosLang);
        Task<OperationResult> Delete(int IDPosition);
        Task<PaginationUtility<LunchBreakDto>> GetDataPagination(PaginationParam pagination, bool isPaging);
        Task<LunchBreakDto> GetDetail(int IDPosition);
        Task<List<LunchBreakDto>> GetListLunchBreak();
    }
}