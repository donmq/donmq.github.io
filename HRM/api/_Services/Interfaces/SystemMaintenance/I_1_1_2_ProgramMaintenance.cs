

using API.DTOs.SystemMaintenance;

namespace API._Services.Interfaces.SystemMaintenance
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_1_1_2_ProgramMaintenance
    {
        Task<List<KeyValuePair<string, string>>> GetDirectory();
        Task<PaginationUtility<ProgramMaintenance_Data>> Getdata(PaginationParam pagination, ProgramMaintenance_Param param);
        Task<List<KeyValuePair<string, string>>> GetFunction_ALL();
        Task<OperationResult> AddNew(ProgramMaintenance_Data model);
        Task<OperationResult> Edit(ProgramMaintenance_Data model);
        Task<OperationResult> Delete(string Program_Code);
    }
}