using API.Dtos.Common;
using API.Dtos.SeaHr;
using API.Helpers.Params;
using API.Helpers.Utilities;

namespace API._Services.Interfaces.SeaHr
{
    [DependencyInjectionAttribute(ServiceLifetime.Scoped)]
    public interface ISeaConfirmService
    {
        Task<SeaConfirmSearchDto> Search(SeaConfirmParam param, PaginationParam pagination, bool isPaging = true);
        Task<List<KeyValueUtility>> GetCategories();
        Task<List<KeyValueUtility>> GetParts(int deptID);
        Task<SeaConfirmEmpDetailDto> GetEmpDetail(int empID);
        Task<SeaConfirmEmpDetailDto> GetLeaveDeleteTopFive(int empID);
        Task<OperationResult> Confirm(List<LeaveDataDto> data, string username);
        Task<List<KeyValueUtility>> GetDepartments();
        Task<OperationResult> DownloadExcel(SeaConfirmParam param, PaginationParam pagination);

    }
}