using Aspose.Cells;
using Machine_API.DTO;
using Machine_API.Helpers.Attributes;
using Machine_API.Helpers.Params;

namespace Machine_API._Service.interfaces
{
    [DependencyInjectionAttribute(ServiceLifetime.Scoped)]
    public interface IInventoryService
    {
        Task<SearchMachineInventoryDto> GetMachine(string idMachine, string lang);
        Task<List<SearchMachineInventoryDto>> GetAllMachineByPlno(string plnoID);
        Task<ResultHistoryInventoryDto> SubmitInventory(InventoryParams inventoryParams, string userName, string empName);
        void PutStaticValue(ref Worksheet ws, ResultHistoryInventoryDto data, string userName, string empName);
        void CustomStyle(ref Cell cellCustom);
    }
}