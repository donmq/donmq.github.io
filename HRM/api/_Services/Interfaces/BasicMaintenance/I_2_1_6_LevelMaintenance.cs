
using API.DTOs.BasicMaintenance;

namespace API._Services.Interfaces.BasicMaintenance
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_2_1_6_LevelMaintenance
    {
        Task<PaginationUtility<HRMS_Basic_LevelDto>> GetData(PaginationParam pagination, LevelMaintenanceParam param, bool isPaging = true);
        Task<OperationResult> Delete(HRMS_Basic_LevelDto model);
        Task<OperationResult> Add(HRMS_Basic_LevelDto model);
        Task<OperationResult> Edit(HRMS_Basic_LevelDto model);
        Task<List<KeyValuePair<string, string>>> GetListLevelCode(string type, string language); 
        Task<OperationResult> ExportExcel(LevelMaintenanceParam param, string lang);
        Task<List<KeyValuePair<string, string>>> GetTypes(string language);
      
    }
}