using API.Dtos.Common;
using API.Dtos.SeaHr.EditLeave;
namespace API._Services.Interfaces.SeaHr
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface IEditLeaveService
    {
        Task<PaginationUtility<LeaveDataDto>> GetAllEditLeave(PaginationParam param);
        Task<OperationResult> AcceptEditLeave(int LeaveID, string UserName);
        Task<DetailEmployeeDto> GetDetailEmployee(int EmployeeID);
        Task<OperationResult> RejectEditLeave(int LeaveID, string UserName);
    }
}
