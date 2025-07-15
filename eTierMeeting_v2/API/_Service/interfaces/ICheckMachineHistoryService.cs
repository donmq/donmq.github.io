using Aspose.Cells;
using Machine_API.DTO;
using Machine_API.Helpers.Attributes;
using Machine_API.Helpers.Params;
using Machine_API.Helpers.Utilities;

namespace Machine_API._Service.interfaces
{
    [DependencyInjectionAttribute(ServiceLifetime.Scoped)]
    public interface ICheckMachineHistoryService
    {
        Task<List<DataHistoryCheckMachineDto>> GetDetailHistoryCheckMachine(int historyCheckMachineID);
        Task<PageListUtility<HistoryCheckMachineDto>> SearchHistoryCheckMachine(CheckMachineHisstoryParams checkMachineHisstoryParams,PaginationParams paginationParams);
        void PutStaticValue(ref Worksheet ws, HistoryCheckMachineDto data);
        void CustomStyle(ref Cell cellCustom);
    }
}