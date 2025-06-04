using API.Dtos.Common;
using API.Helpers.Params.SeaHr.EmployeeManager;
namespace API._Services.Interfaces.SeaHr
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface ISeaEmpManagerServices
    {
        Task<PaginationUtility<HistoryEmpDto>> Search(PaginationParam param, SeaEmployeeFilter filter);

        Task<List<KeyValuePair<int, string>>> GetAreas();

        Task<List<KeyValuePair<int, string>>> GetDepartments(int areaId);

        Task<List<KeyValuePair<int, string>>> GetParts(int departmentId);
    }
}