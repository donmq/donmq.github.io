using eTierV2_API.DTO;
using eTierV2_API.Helpers.Params;
using eTierV2_API.Helpers.Utilities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eTierV2_API._Services.Interfaces.Production.T1.C2B
{
    public interface IT2MeetingTimeSettingService
    { 
        Task<PaginationUtility<eTM_T2_Meeting_SeetingDTO>> GetAllData(Helpers.Params.PaginationParam pagination, T2MeetingTimeSettingParam param);
        Task<List<KeyValuePair<string, string>>> GetListBuildingOrGroup(); 
        Task<OperationResult> Add(eTM_T2_Meeting_SeetingDTO eTM_T2_Meeting_SeetingDTO);
        Task<OperationResult> Delete(eTM_T2_Meeting_SeetingDTO eTM_T2_Meeting_SeetingDTO); 
        
    }

} 