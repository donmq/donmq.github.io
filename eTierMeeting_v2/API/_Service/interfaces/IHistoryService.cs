using Machine_API.DTO;
using Machine_API.Helpers.Attributes;
using Machine_API.Helpers.Params;
using Machine_API.Helpers.Utilities;

namespace Machine_API._Service.interfaces
{
    [DependencyInjectionAttribute(ServiceLifetime.Scoped)]
    public interface IHistoryService
    {
        Task<PageListUtility<HistoryDto>> SearchHistory(PaginationParams paginationParams, SearchHistoryParams searchHistoryParams);

        Task<List<HistoryExelDto>> ExcelHistories(PaginationParams paginationParams, SearchHistoryParams searchHistoryParams);
    }
}