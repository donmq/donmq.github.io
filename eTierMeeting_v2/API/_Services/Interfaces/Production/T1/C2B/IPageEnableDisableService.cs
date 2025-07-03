using System.Collections.Generic;
using System.Threading.Tasks;
using eTierV2_API.DTO;
using eTierV2_API.Helpers.Params;
using eTierV2_API.Helpers.Utilities;

namespace eTierV2_API._Services.Interfaces.Production.T1.C2B
{
    public interface IPageEnableDisableService
    {
        Task<List<KeyValuePair<string, string>>> GetCenters();
        Task<List<KeyValuePair<string, string>>> GetTiers(string center_Level);
        Task<List<KeyValuePair<string, string>>> GetSections(string center_Level, string tier_Level);
        Task<List<eTM_Page_SettingsDTO>> GetPages(PageEnableDisableParam param);
        Task<OperationResult> UpdatePages(List<eTM_Page_SettingsDTO> pagesDto);
    }
}