using API.Dtos.SeaHr;
using API.Dtos.SeaHr.SeaHrHistory;
namespace API._Services.Interfaces.SeaHr
{
    [DependencyInjectionAttribute(ServiceLifetime.Scoped)]
    public interface ISeaHrHistoryService
    {
        Task<List<KeyValuePair<int, string>>> GetCategory(string lang);
        Task<List<KeyValuePair<int, string>>> GetDepartments();
        Task<List<KeyValuePair<int, string>>> GetPart(int ID);
        Task<SeaHistorySearchDto> GetLeaveData(SearchHistoryParamsDto paramsSearch, PaginationParam pagination, bool isPaging = true);
        Task<OperationResult> ExportExcel(PaginationParam pagination, SearchHistoryParamsDto dto);
    }
}