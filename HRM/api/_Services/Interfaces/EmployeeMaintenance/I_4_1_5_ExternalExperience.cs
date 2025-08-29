using API.DTOs.EmployeeMaintenance;

namespace API._Services.Interfaces.EmployeeMaintenance
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_4_1_5_ExternalExperience
    {
        Task<OperationResult> AddNew(HRMS_Emp_External_ExperienceDto data, string userName);
        Task<OperationResult> Edit(HRMS_Emp_External_ExperienceDto data, string userName);
        Task<OperationResult> Delete(HRMS_Emp_External_ExperienceDto data);
        Task<int> GetMaxSeq(HRMS_Emp_External_ExperienceDto data);
        Task<List<HRMS_Emp_External_ExperienceDto>> GetData(HRMS_Emp_External_ExperienceParam filter);
    }
}