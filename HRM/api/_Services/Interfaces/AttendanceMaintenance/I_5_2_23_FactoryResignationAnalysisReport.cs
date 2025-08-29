

using API.DTOs.AttendanceMaintenance;

namespace API._Services.Interfaces.AttendanceMaintenance
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_5_2_23_FactoryResignationAnalysisReport
    {
        Task<List<KeyValuePair<string, string>>> GetDropDownList(FactoryResignationAnalysisReport_Param param, List<string> roleList);
        Task<OperationResult> Process(FactoryResignationAnalysisReport_Param param, string userName);
    }
}