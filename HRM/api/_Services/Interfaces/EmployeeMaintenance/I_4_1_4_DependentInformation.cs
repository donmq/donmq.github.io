using API.DTOs.EmployeeMaintenance;

namespace API._Services.Interfaces.EmployeeMaintenance
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_4_1_4_DependentInformation
    {
        Task<List<KeyValuePair<string, string>>> GetListRelationship(string language);
        Task<List<HRMS_Emp_DependentDto>> GetData(HRMS_Emp_DependentParam param);
        Task<OperationResult> AddNew(HRMS_Emp_DependentDto model, string userName);
        Task<OperationResult> Edit(HRMS_Emp_DependentDto model, string userName);
        Task<OperationResult> Delete(HRMS_Emp_DependentDto model);
        Task<int> GetMaxSeq(HRMS_Emp_DependentDto model);
    }
}