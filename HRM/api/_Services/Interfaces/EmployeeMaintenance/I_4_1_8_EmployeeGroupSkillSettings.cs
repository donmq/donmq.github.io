using API.DTOs.EmployeeMaintenance;

namespace API._Services.Interfaces.EmployeeMaintenance
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_4_1_8_EmployeeGroupSkillSettings
    {
        Task<List<KeyValuePair<string, string>>> GetDropDownList(EmployeeGroupSkillSettings_Param param);
        Task<PaginationUtility<EmployeeGroupSkillSettings_Main>> GetSearchDetail(PaginationParam paginationParams, EmployeeGroupSkillSettings_Param searchParam, List<string> roleList);
        Task<List<KeyValuePair<string, string>>> GetEmployeeList(EmployeeGroupSkillSettings_Param param);
        Task<OperationResult> CheckExistedData(EmployeeGroupSkillSettings_Param param);
        Task<OperationResult> PostData(EmployeeGroupSkillSettings_Main data, string userName);
        Task<OperationResult> PutData(EmployeeGroupSkillSettings_Main data, string userName);
        Task<OperationResult> DeleteData(string division, string factory, string employee_Id);
    }
}