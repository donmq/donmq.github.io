using API.DTOs.EmployeeMaintenance;

namespace API._Services.Interfaces.EmployeeMaintenance
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_4_1_7_IdentificationCardToEmployeeIDHistory
    {
        Task<List<KeyValuePair<string, string>>> GetListDivision(string language);
        Task<List<KeyValuePair<string, string>>> GetListFactory(string division, string language);
        Task<List<KeyValuePair<string, string>>> GetListNationality(string language);
        Task<PaginationUtility<HRMS_Emp_IDcard_EmpID_HistoryDto>> GetDataPagination(PaginationParam pagination, IdentificationCardToEmployeeIDHistoryParam param);
    }
}