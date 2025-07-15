using Machine_API.DTO;
using Machine_API.Helpers.Attributes;
using Machine_API.Helpers.Params;

namespace Machine_API._Service.interfaces
{
    [DependencyInjectionAttribute(ServiceLifetime.Scoped)]
    public interface IMachineReportService
    {
         Task<ReportMachineDto> GetListReportMachine(SearchMachineParams searchMachineParams);
    }
}