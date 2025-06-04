using API.Dtos.SeaHr;
namespace API._Services.Interfaces.SeaHr
{
    [DependencyInjectionAttribute(ServiceLifetime.Scoped)]
    public interface ISeaHrAddEmployeeService
    {
        Task<List<KeyValuePair<int, string>>> GetListDepartment();
        Task<List<ListPositionDTO>> GetListPosition();
        Task<List<KeyValuePair<int, string>>> GetListPart(int partID);
        Task<List<ListGroupBaseDTO>> GetListGroupBase();
        Task<OperationResult> AddNewEmployee(EmployeeDTO employeeDTO);
        Task<OperationResult> UploadExcel(IFormFile file);
    }
}