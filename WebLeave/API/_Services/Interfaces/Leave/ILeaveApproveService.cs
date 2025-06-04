using API.Dtos.Leave.LeaveApprove;
namespace API._Services.Interfaces.Leave
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface ILeaveApproveService
    {
        Task<List<KeyValuePair<int, string>>> GetCategory(string lang);
        Task<PaginationUtility<LeaveDataApproveDto>> GetLeaveData(SearchLeaveApproveDto paramsSearch, PaginationParam pagination, bool isPaging = true);
        Task<OperationResult> UpdateLeaveData(List<LeaveDataApproveDto> models, Boolean check = true);
        Task<OperationResult> ExportExcel(PaginationParam pagination, SearchLeaveApproveDto dto);
    }
}