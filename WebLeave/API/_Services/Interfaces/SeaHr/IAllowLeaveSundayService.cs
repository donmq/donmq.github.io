using API.Dtos.SeaHr;

namespace API._Services.Interfaces.SeaHr
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface IAllowLeaveSundayService
    {
        Task<List<KeyValuePair<int, string>>> GetParts();
        Task<PaginationUtility<AllowLeaveSundayDto>> GetPagination(PaginationParam pagination, AllowLeaveSundayParam param);
        Task<List<AllowLeaveSundayDto>> GetEmployee(AllowLeaveSundayParam param);
        Task<OperationResult> AllowLeave(List<int> EmpSelected);
        Task<OperationResult> DisallowLeave(int EmpID);
    }
}