using Aspose.Cells;
using Machine_API.DTO;
using Machine_API.Helpers.Attributes;
using Machine_API.Helpers.Utilities;

namespace Machine_API._Service.interfaces
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface IReportCheckMachineSafetyService
    {
        Task<OperationResult> ExportExcel(ReportCheckMachineSafetyParam param);
    }
}