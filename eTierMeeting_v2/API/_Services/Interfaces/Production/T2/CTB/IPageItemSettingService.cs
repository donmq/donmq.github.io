using System.Collections.Generic;
using System.Threading.Tasks;
using eTierV2_API.DTO;
using eTierV2_API.Helpers.Params.Production.T2.CTB;
using eTierV2_API.Helpers.Utilities;

namespace eTierV2_API._Services.Interfaces.Production.T2.CTB
{
    public interface IPageItemSettingService
    {
        Task<PaginationUtility<eTM_Page_Item_SettingsDTO>> GetAll(PageItemSettingParam param, PaginationParam pagination);
        Task<OperationResult> Add(eTM_Page_Item_SettingsDTO settingDTO, string update_By);
        Task<OperationResult> Update(eTM_Page_Item_SettingsDTO settingDTO, string update_By);
        Task<List<KeyValuePair<string, string>>> GetCenterLevels();
        Task<List<KeyValuePair<string, string>>> GetTierLevels(string center_Level);
        Task<List<KeyValuePair<string, string>>> GetSections(string center_Level, string tier_Level);
        Task<List<KeyValuePair<string, string>>> GetPages();
    }
}