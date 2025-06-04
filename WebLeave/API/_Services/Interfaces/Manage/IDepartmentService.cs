using API.Dtos;
using API.Helpers.Params;
namespace API._Services.Interfaces.Manage
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface IDepartmentService
    {
        Task<PaginationUtility<DepartmentDto>> GetAllDepartment(PaginationParam pagination, DepartmentParams search);
        Task<List<KeyValuePair<int, string>>> GetAllAreas();
        Task<List<KeyValuePair<int, string>>> GetAllBuildings();
        Task<OperationResult> Add(DepartmentDto departmentDto);
        Task<OperationResult> Update(DepartmentDto departmentDto);

    }
}