using API.DTOs.EmployeeMaintenance;

namespace API._Services.Interfaces.EmployeeMaintenance
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_4_1_13_ExitEmployeesBlacklist
    {
        Task<List<KeyValuePair<string, string>>> GetListNationality(string language);
        Task<List<KeyValuePair<string, string>>> GetIdentificationNumber();
        Task<OperationResult> Edit(HRMS_Emp_BlacklistDto model, string userName);
        Task<PaginationUtility<HRMS_Emp_BlacklistDto>> GetDataPagination(PaginationParam pagination, HRMS_Emp_BlacklistParam param);
    }
}