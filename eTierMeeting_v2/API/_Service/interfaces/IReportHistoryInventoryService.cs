using Aspose.Cells;
using Machine_API.DTO;
using Machine_API.Helpers.Attributes;

namespace Machine_API._Service.interfaces
{
    [DependencyInjectionAttribute(ServiceLifetime.Scoped)]
    public interface IReportHistoryInventoryService
    {
        void PutStaticValue(ref Worksheet ws, ResultAllInventoryDto resultAll);
        void PutStaticAllInventoryValue(ref Worksheet ws, ResultAllInventoryDto resultAll);

        void CustomStyle(ref Cell cellCustom);
    }
}