using Machine_API.DTO;
using Machine_API.Helpers.Attributes;
using Machine_API.Helpers.Params;
using Machine_API.Helpers.Utilities;

namespace Machine_API._Service.interfaces
{
    [DependencyInjectionAttribute(ServiceLifetime.Scoped)]
    public interface IDateInventoryServive
    {
        Task<PageListUtility<DateInventoryDto>> GetAllDateInventories(PaginationParams paginationParams);
        Task<DateInventoryDto> GetDateInventory(int id);
        Task<OperationResult> AddDateInventory(AddDateDto addDate, string userName, string empName, string lang);
        Task<OperationResult> RemoveDateInventory(int id, string lang);
        Tuple<bool, DateTime?> CheckScheduleInventory();
    }
}