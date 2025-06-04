using API.Dtos.Leave;
namespace API._Services.Interfaces.Leave
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface IEditLeaveDataService
    {
        Task<OperationResult> EditLeaveData(int LeaveID, string UserName);
        Task<PaginationUtility<LeaveDataDTO>> GetAllEditLeave(PaginationParam param, int userID);
        Task<DetailDTO> GetDetailEmployee(string leaveID);
        Task<LeaveDataDTO> GetLeaveByID(string leaveID);
    }
}