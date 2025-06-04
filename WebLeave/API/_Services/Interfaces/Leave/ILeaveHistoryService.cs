using API.Dtos.Leave.LeaveHistory;
namespace API._Services.Interfaces.Leave
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface ILeaveHistoryService
    {
        Task<List<KeyValuePair<int, string>>> GetCategory(string lang);
        Task<LeaveDataDtos> GetLeaveData(SearchHistoryParamsDto paramsSearch, PaginationParam pagination, bool isPaging = true);
        Task<OperationResult> ExportExcel(HistoryExportParam param, PaginationParam pagination);
    }
}