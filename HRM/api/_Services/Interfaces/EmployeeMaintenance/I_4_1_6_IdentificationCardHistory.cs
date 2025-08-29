using API.DTOs.EmployeeMaintenance;

namespace API._Services.Interfaces.EmployeeMaintenance
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_4_1_6_IdentificationCardHistory
    {
        Task<List<HRMS_Emp_Identity_Card_HistoryDto>> GetData(HRMS_Emp_Identity_Card_HistoryParam param);
        Task<OperationResult> Create(HRMS_Emp_Identity_Card_HistoryDto dto);
        Task<List<KeyValuePair<string, string>>> GetListNationality(string Language);
        Task<List<string>> GetListTypeHeadIdentificationNumber(string nationality);
    }
}