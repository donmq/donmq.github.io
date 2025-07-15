using Aspose.Cells;
using Machine_API.DTO;
using Machine_API.Helpers.Attributes;
using Machine_API.Models.MachineCheckList;

namespace Machine_API._Service.interfaces
{
    [DependencyInjectionAttribute(ServiceLifetime.Scoped)]
    public interface ICheckMachineService
    {
        Task<object> GetMachine(string idMachine, string lang);
        Task<List<CheckMachineDto>> GetAllMachine();
        Task<ResultHistoryCheckMachineDto> SubmitCheckMachineAll(List<CheckMachineDto> listModel, string userName, string empName);
        Task<ResultHistoryCheckMachineDto> SubmitCheckMachine(List<CheckMachineDto> listModel, string userName, string empName);
        IEnumerable<DataHistoryCheckMachine> GetListDataHistoryById(int id);
        HistoryCheckMachine GetHistoryCheckMachine(int id);
        void PutStaticValue(ref Worksheet ws, ResultHistoryCheckMachineDto data, string userName, string empName);
        void CustomStyle(ref Cell cellCustom);
    }
}