using Machine_API.DTO;
using Machine_API.Helpers.Attributes;
using Machine_API.Helpers.Params;
using Machine_API.Helpers.Utilities;

namespace Machine_API._Service.interfaces
{
    [DependencyInjectionAttribute(ServiceLifetime.Scoped)]
    public interface IDataHistoryCheckMachineService
    {
        Task<PageListUtility<SearchMachineDto>> SearchMachine(PaginationParams paginationParams, SearchMachineParams searchMachineParams);
        Task<List<SearchMachineDto>> ExportExcelMachine(PaginationParams paginationParams, SearchMachineParams searchMachineParams);
    }
}